using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestfulAPIService.Models;

namespace RestfulAPIService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : Controller
    {
        #region Fields
        public ObservableCollection<ArticleContext> _context;
        #endregion


        #region Methods
        public async Task<List<ArticleContext>> GetArticleFromApiAsync(string uri, List<ArticleContext> actionList)
        {
            List<Product> productList = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    foreach (var product in productList)
                    {
                        foreach (var article in product.articles)
                            actionList.Add(new ArticleContext(product, article));
                    }
                }
            }
            return actionList;
        }
        public List<ArticleContext> GetMinandMaxPrice(List<ArticleContext> actionList)
        {
            var filtertActionList = new List<ArticleContext>();
            var listPricePerLiter = new List<double>(actionList.Select(s => double.Parse(s.pricePerUnitText.Replace("(", "").Replace("€", "").Replace("Liter", "").Replace(")", "").Replace("/", "").Replace(" ", ""))).ToList());
            double maxValue = double.MinValue;
            foreach (object item in listPricePerLiter)
                if ((double)item > maxValue)
                    maxValue = (double)item;

            double min = double.MaxValue;
            foreach (object item in listPricePerLiter)
                if (min > (double)item)
                    min = (double)item;

            foreach (var item in actionList.Where(s => double.Parse(s.pricePerUnitText.Replace("(", "").Replace("€", "").Replace("Liter", "").Replace(")", "").Replace("/", "").Replace(" ", "")) == min).ToList())
                filtertActionList.Add(item);
            foreach (var item in actionList.Where(s => double.Parse(s.pricePerUnitText.Replace("(", "").Replace("€", "").Replace("Liter", "").Replace(")", "").Replace("/", "").Replace(" ", "")) == maxValue).ToList())
                filtertActionList.Add(item);
            return filtertActionList;
        }
        public string SerializeListArticleContext(List<ArticleContext> resultList)
        {
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = System.Text.Json.JsonSerializer.Serialize<IList<ArticleContext>>(resultList, opt);
            return strJson;
        }
        public List<ArticleContext> SortByAscending(List<ArticleContext> actionList)
        {
            var filtertActionList = new List<ArticleContext>();
            filtertActionList = actionList.OrderBy(i => i.pricePerUnitText).ToList();
                return filtertActionList;
        }
        public List<ArticleContext> FindePrice(double? price, List<ArticleContext> actionList)
        {
            var filtertActionList = new List<ArticleContext>();
            var result = actionList.Where(s => s.price == price);
            foreach (var item in result)
                filtertActionList.Add(item);
                return filtertActionList;
        }
        public List<ArticleContext> FindeMostUnits(List<ArticleContext> actionList)
        {
            var filtertActionList = new List<ArticleContext>();
            int maxValue = actionList.Select(s => int.Parse(Regex.Replace(s.shortDescription.Split()[0], @"[^0-9a-zA-Z\ ]+", ""))).Max();
            var result = actionList.Where(s => s.shortDescription.Contains(maxValue.ToString())).ToList();
            foreach (var item in result)
                filtertActionList.Add(item);
            return filtertActionList;
        }
        public string ErrorForUri(){
            return "Uri is Empty. Please fill the Uri";
            }
        #endregion

        #region ApiRoutes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet("MinAndMaxPricePerLiter")]
        public async Task<string> MinAndMaxPricePerLiter(bool isMinAndMaxPricePerLiter, string uri)
        {         
            List<ArticleContext> actionList = new List<ArticleContext>();
            string jsonMinAndMaxPricePerLiter = String.Empty;
            string jsonArticleList = String.Empty;
            if (String.IsNullOrEmpty(uri))
                return ErrorForUri();
            await GetArticleFromApiAsync(uri, actionList);

            if (isMinAndMaxPricePerLiter)
            {
                var resultList = GetMinandMaxPrice(actionList);
               jsonMinAndMaxPricePerLiter = "\r\n\r\nMinAndMaxPricePerLiter\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            else
                jsonArticleList = "\r\n\r\nAllArticles\r\n\r\n" + SerializeListArticleContext(actionList);
     
            return jsonMinAndMaxPricePerLiter + jsonArticleList;
        }

        [HttpGet("ByPriceAndSortAscending")]
        public async Task<string> ByPriceAndSortAscending(bool isSortByAscending, double? price, string uri)
        {
            List<ArticleContext> actionList = new List<ArticleContext>();
            string jsonSortByAscending = String.Empty;
            string jsonFindePrice = String.Empty;
            string jsonArticleList = String.Empty;
            if (String.IsNullOrEmpty(uri))
                return ErrorForUri();
            await GetArticleFromApiAsync(uri, actionList);
            if (price != null && price != 0)
            {
                var resultList = FindePrice(price, actionList);
                jsonFindePrice = "\r\n\r\nFindePrice: "+ price+ "\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            if (isSortByAscending)
            {
                var resultList = SortByAscending(actionList);
                jsonSortByAscending = "\r\n\r\nSortByAscending\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            else
                jsonArticleList = "\r\n\r\nAllArticles\r\n\r\n" + SerializeListArticleContext(actionList);

            return jsonFindePrice + jsonSortByAscending + jsonArticleList;
        }

        [HttpGet("MostUnits")]
        public async Task<string> MostUnits(bool isMostUnits, string uri)
        {
            List<ArticleContext> actionList = new List<ArticleContext>();
            string jsonMostUnits = String.Empty;
            string jsonArticleList = String.Empty;
            if (String.IsNullOrEmpty(uri))
                return ErrorForUri();
            await GetArticleFromApiAsync(uri, actionList);

            if (isMostUnits)
            {
                var resultList = FindeMostUnits(actionList);
                jsonMostUnits = "\r\n\r\nMostUnits\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            else
                jsonArticleList = "\r\n\r\nAllArticles\r\n\r\n" + SerializeListArticleContext(actionList);

            return jsonMostUnits + jsonArticleList;
        }

        [HttpGet("GetAll")]
        public async Task<string> GetAll(double? price, bool isMostUnits, bool isSortByAscending, bool isMinAndMaxPricePerLiter, string uri)
        {
            List<ArticleContext> actionList = new List<ArticleContext>();
            string jsonSortByAscending = String.Empty;
            string jsonMostUnits = String.Empty;
            string jsonFindePrice = String.Empty;
            string jsonMinAndMaxPricePerLiter = String.Empty;
            if (String.IsNullOrEmpty(uri))
                return ErrorForUri();
            await GetArticleFromApiAsync(uri, actionList);
            if (isMinAndMaxPricePerLiter)
            {
                var resultList = GetMinandMaxPrice(actionList);
                jsonMinAndMaxPricePerLiter = "\r\n\r\nMinAndMaxPricePerLiter\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            if (price != null && price != 0)
            {
                var resultList = FindePrice(price, actionList);
                jsonFindePrice = "\r\n\r\nFindePrice: " + price + "\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            if (isMostUnits)
            {
                var resultList = FindeMostUnits(actionList);
                jsonMostUnits = "\r\n\r\nMostUnits\r\n\r\n" + SerializeListArticleContext(resultList);
            }
            if (isSortByAscending)
            {
                var resultList = SortByAscending(actionList);
                jsonSortByAscending = "\r\n\r\nSortByAscending\r\n\r\n" + SerializeListArticleContext(resultList);
            }


            return jsonMinAndMaxPricePerLiter + jsonFindePrice + jsonSortByAscending  + jsonMostUnits;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIService.Models
{

    public class ArticleContext 
    {
        public int Id { get; set; }
        public int idProduct { get; set; }
        public string brandName { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public double price { get; set; }
        public string unit { get; set; }
        public string pricePerUnitText { get; set; }
        public string image { get; set; }
  

        public ArticleContext() { }
        public ArticleContext(Product product, Article article)
        {
            Id = article.id;
            idProduct = product.id;
            shortDescription = article.shortDescription;
            price = article.price;
            unit = article.unit;
            pricePerUnitText = article.pricePerUnitText;
            image = article.image;
            brandName = product.brandName;
            name = product.name;
        }
    }
}

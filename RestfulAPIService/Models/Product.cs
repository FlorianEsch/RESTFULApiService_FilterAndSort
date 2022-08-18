using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIService.Models
{
    public class Product
    {
        public int id { get; set; }
        public string brandName { get; set; }
        public string name { get; set; }
        public string descriptionText { get; set; }
        public List<Article> articles;

        public Product()
        {
        }
    }
}

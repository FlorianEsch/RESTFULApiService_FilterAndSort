using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIService.Models
{
    public class Article
    {
        public int id { get; set; }
        public string shortDescription { get; set; }
        public double price { get; set; }
        public string unit { get; set; }
        public string pricePerUnitText { get; set; }
        public string image { get; set; }
    }
}

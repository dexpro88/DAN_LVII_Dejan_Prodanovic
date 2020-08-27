using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArticleService.Model
{
    public class Article
    {
        public string ID { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
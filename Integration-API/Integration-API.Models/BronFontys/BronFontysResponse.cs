using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_API.Models.BronFontys
{
    public class BronFontysResponse
    {
        public string title { get; set; }
        public string link { get; set; }
        public string imgUrl { get; set; }
        public string description { get; set; }
        public string shortDescription { get; set; }
        public DateTime pubDate { get; set; }
    }
}

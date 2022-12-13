using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Integration_API.Models.BronFontys
{
    public class BronFontysCredentials
    {
        [BsonElement("Active")]
        public bool Active { get; set; }

        public BronFontysCredentials(bool Active)
        {
            this.Active = Active;
        }
    }
}

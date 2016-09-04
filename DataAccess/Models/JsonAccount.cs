using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonAccount
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Uri URL { get; set; }
        public Uri Image { get; set; }
        public string RealName { get; set; }
        public string CountryCode { get; set; }

        public JsonAccount(JToken json)
        {
            ID = (long)json["steamid"];
            Name = (string)json["personaname"];
            URL = new Uri((string)json["profileurl"]);
            Image = new Uri((string)json["avatar"]);
            if (json["realname"] != null)
            {
                RealName = (string)json["realname"];
            }
            if (json["loccountrycode"] != null)
            {
                CountryCode = (string)json["loccountrycode"];
            }
        }
    }
}

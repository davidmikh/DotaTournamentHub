using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonUser
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        public int HeroID { get; set; }
        public int Team { get; set; }

        public JsonUser(JToken json)
        {
            AccountID = (int)json["account_id"];
            Name = (string)json["name"];
            HeroID = (int)json["hero_id"];
            Team = (int)json["team"];
        }
    }
}

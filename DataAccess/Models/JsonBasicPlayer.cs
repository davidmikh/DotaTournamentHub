using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonBasicPlayer
    {
        public long AccountID { get; set; }
        public int Slot { get; set; }
        public int HeroID { get; set; }

        public JsonBasicPlayer(JToken json)
        {
            AccountID = (long)json["account_id"];
            Slot = (int)json["player_slot"];
            HeroID = (int)json["hero_id"];
        }
    }
}

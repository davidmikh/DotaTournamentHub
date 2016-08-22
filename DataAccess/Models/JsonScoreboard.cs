using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonScoreboard
    {
        public int Duration { get; set; }
        public JsonTeam Radiant { get; set; }
        public JsonTeam Dire { get; set; }

        public JsonScoreboard(JToken json)
        {
            if (json != null)
            {
                Duration = (int)json["duration"];
                Radiant = new JsonTeam(json["radiant"]);
                Dire = new JsonTeam(json["dire"]);
            }
        }
    }
}

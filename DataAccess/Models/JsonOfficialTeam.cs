using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonOfficialTeam
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public long Logo { get; set; }
        //Indicates whether the players for this team are all team members
        public bool Complete { get; set; }

        public JsonOfficialTeam(JToken json)
        {
            if (json != null)
            {
                Name = (string)json["team_name"];
                ID = (long)json["team_id"];
                Logo = (long)json["team_logo"];
                Complete = (bool)json["complete"];
            }
        }
    }
}

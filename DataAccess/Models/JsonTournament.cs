using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonTournament
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public int TicketItemID { get; set; }

        public JsonTournament(JToken json)
        {
            Name = ((string)json["name"]).Substring(11);
            ID = (int)json["leagueid"];
            Description = (string)json["description"];
            URL = (string)json["tournament_url"];
            TicketItemID = (int)json["itemdef"];
        }
    }
}

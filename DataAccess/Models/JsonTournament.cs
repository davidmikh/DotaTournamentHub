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
        public long ID { get; set; }
        public string Description { get; set; }
        public long TicketItemID { get; set; }

        public JsonTournament(JToken json)
        {
            Name = ((string)json["name"]).Substring(11);
            ID = (long)json["leagueid"];
            Description = (string)json["description"];
            TicketItemID = (long)json["itemdef"];
        }
    }
}

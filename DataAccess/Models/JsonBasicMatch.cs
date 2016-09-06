using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonBasicMatch
    {
        public List<JsonBasicPlayer> Players { get; set; }
        public long SeriesID { get; set; }
        public int SeriesType { get; set; }
        public long ID { get; set; }
        public long SequenceNum { get; set; }
        public DateTime StartTime { get; set; }
        public int LobbyType { get; set; }
        public long RadiantTeamID { get; set; }
        public long DireTeamID { get; set; }

        public JsonBasicMatch(JToken json)
        {
            Players = new List<JsonBasicPlayer>();
            if (json["players"] != null)
            {
                foreach (var player in json["players"])
                {
                    Players.Add(new JsonBasicPlayer(player));
                }
            }
            SeriesID = (long)json["series_id"];
            SeriesType = (int)json["series_type"];
            ID = (long)json["match_id"];
            SequenceNum = (long)json["match_seq_num"];
            StartTime = DateTimeOffset.FromUnixTimeSeconds((long)json["start_time"]).UtcDateTime;
            LobbyType = (int)json["lobby_type"];
            RadiantTeamID = (long)json["radiant_team_id"];
            DireTeamID = (long)json["dire_team_id"];
        }
    }
}

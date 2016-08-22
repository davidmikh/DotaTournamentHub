using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonMatch
    {
        public List<JsonUser> Users { get; set; }
        public JsonOfficialTeam Radiant { get; set; }
        public JsonOfficialTeam Dire { get; set; }
        public long LobbyID { get; set; }
        public long MatchID { get; set; }
        public int NumSpectators { get; set; }
        public long SeriesID { get; set; }
        public int GameNumber { get; set; }
        public long LeagueID { get; set; }
        public int RadiantSeriesWins { get; set; }
        public int DireSeriesWins { get; set; }
        public int SeriesType { get; set; }
        public long LeagueSeriesID { get; set; }
        public long LeagueGameID { get; set; }
        public string StageName { get; set; }
        public int LeagueTier { get; set; }
        public JsonScoreboard ScoreBoard { get; set; }

        public JsonMatch(JToken json)
        {
            Users = new List<JsonUser>();
            foreach (var user in json["players"])
            {
                Users.Add(new JsonUser(user));
            }
            Radiant = new JsonOfficialTeam(json["radiant_team"]);
            Dire = new JsonOfficialTeam(json["dire_team"]);
            LobbyID = (long)json["lobby_id"];
            MatchID = (long)json["match_id"];
            NumSpectators = (int)json["spectators"];
            SeriesID = (long)json["series_id"];
            GameNumber = (int)json["game_number"];
            LeagueID = (long)json["league_id"];
            RadiantSeriesWins = (int)json["radiant_series_wins"];
            DireSeriesWins = (int)json["dire_series_wins"];
            SeriesType = (int)json["series_type"];
            LeagueSeriesID = (long)json["league_series_id"];
            LeagueGameID = (long)json["league_game_id"];
            StageName = (string)json["stage_name"];
            LeagueTier = (int)json["league_tier"];
            ScoreBoard = new JsonScoreboard(json["scoreboard"]);
        }
    }
}

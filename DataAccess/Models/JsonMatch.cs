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
        public List<JsonPlayer> Players { get; set; }
        public bool RadiantWin { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public long ID { get; set; }
        public long SequenceNum { get; set; }
        public int RadiantTowerState { get; set; }
        public int DireTowerState { get; set; }
        public int RadiantBarracksState { get; set; }
        public int DireBarracksState { get; set; }
        public int ServerClusterNum { get; set; }
        public int LobbyType { get; set; }
        public long LeagueID { get; set; }
        public int GameMode { get; set; }
        public int Engine { get; set; }
        public int RadiantKills { get; set; }
        public int DireKills { get; set; }
        public long RadiantTeamID { get; set; }
        public string RadiantName { get; set; }
        public long RadiantLogoID { get; set; }
        public long RadiantCaptainID { get; set; }
        public long DireTeamID { get; set; }
        public string DireName { get; set; }
        public long DireLogoID { get; set; }
        public long DireCaptainID { get; set; }
        public int[] RadiantPicks { get; set; }
        public int[] RadiantBans { get; set; }
        public int[] DirePicks { get; set; }
        public int[] DireBans { get; set; }

        public JsonMatch(JToken json)
        {
            Players = new List<JsonPlayer>();
            foreach (var player in json["players"])
            {
                Players.Add(new JsonPlayer(player));
            }
            RadiantWin = (bool)json["radiant_win"];
            Duration = new TimeSpan(0, 0, (int)json["duration"]);
            StartTime = DateTimeOffset.FromUnixTimeSeconds((long)json["start_time"]).UtcDateTime;
            ID = (long)json["match_id"];
            SequenceNum = (long)json["match_seq_num"];
            RadiantTowerState = (int)json["tower_status_radiant"];
            DireTowerState = (int)json["tower_status_dire"];
            RadiantBarracksState = (int)json["barracks_status_radiant"];
            DireBarracksState = (int)json["barracks_status_dire"];
            ServerClusterNum = (int)json["cluster"];
            LobbyType = (int)json["lobby_type"];
            LeagueID = (long)json["leagueid"];
            GameMode = (int)json["game_mode"];
            Engine = (int)json["engine"];
            RadiantKills = (int)json["radiant_score"];
            DireKills = (int)json["dire_score"];
            if (json["radiant_team_id"] != null)
            {
                RadiantTeamID = (long)json["radiant_team_id"];
                RadiantName = (string)json["radiant_name"];
                RadiantLogoID = (long)json["radiant_logo"];
                RadiantCaptainID = (long)json["radiant_captain"];
            }
            if (json["dire_team_id"] != null)
            {
                DireTeamID = (long)json["dire_team_id"];
                DireName = (string)json["dire_name"];
                DireLogoID = (long)json["dire_logo"];
                DireCaptainID = (long)json["dire_captain"];
            }
            RadiantPicks = json["picks_bans"].Where(t => (bool)t["is_pick"] == true && (int)t["team"] == 0).Select(t => (int)t["hero_id"]).ToArray();
            RadiantBans = json["picks_bans"].Where(t => (bool)t["is_pick"] == false && (int)t["team"] == 0).Select(t => (int)t["hero_id"]).ToArray();
            DirePicks = json["picks_bans"].Where(t => (bool)t["is_pick"] == true && (int)t["team"] == 1).Select(t => (int)t["hero_id"]).ToArray();
            DireBans = json["picks_bans"].Where(t => (bool)t["is_pick"] == false && (int)t["team"] == 1).Select(t => (int)t["hero_id"]).ToArray();
        }
    }
}

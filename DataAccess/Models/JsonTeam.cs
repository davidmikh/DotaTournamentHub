using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonTeam
    {
        public int Score { get; set; }
        public int TowerState { get; set; }
        public int BarracksState { get; set; }
        public int[] HeroPicks { get; set; }
        public int[] HeroBans { get; set; }
        public List<JsonPlayer> Players { get; set; }
        public JsonTeamAbility[] Abilities { get; set; }

        public JsonTeam(JToken json)
        {
            Score = (int)json["score"];
            TowerState = (int)json["tower_state"];
            BarracksState = (int)json["barracks_state"];
            if (json["picks"] != null)
            {
                HeroPicks = new int[5];
                HeroBans = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    if (i < json["picks"].Count())
                    {
                        HeroPicks[i] = (int)json["picks"][i]["hero_id"];
                    }
                    if (i < json["bans"].Count())
                    {
                        HeroBans[i] = (int)json["bans"][i]["hero_id"];
                    }
                }
            }
            Players = new List<JsonPlayer>();
            foreach (var player in json["players"])
            {
                Players.Add(new JsonPlayer(player));
            }
            if (Abilities != null)
            {
                Abilities = new JsonTeamAbility[3];
                for (int i = 0; i < 3; i++)
                {
                    Abilities[i] = new JsonTeamAbility(json["abilities"][i]);
                }
            }
        }
    }

    public class JsonTeamAbility
    {
        public int AbilityID { get; set; }
        public int AbilityLevel { get; set; }

        public JsonTeamAbility(JToken json)
        {
            AbilityID = (int)json["ability_id"];
            AbilityLevel = (int)json["ability_level"];
        }
    }
}

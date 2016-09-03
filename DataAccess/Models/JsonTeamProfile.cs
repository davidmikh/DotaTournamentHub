using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class JsonTeamProfile
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public long LogoID { get; set; }
        public string CountryCode { get; set; }
        public IEnumerable<long> PlayerIDs { get; set; }
        public long CaptainID { get; set; }
        public IEnumerable<long> PlayedTournamentIDs { get; set; }

        public JsonTeamProfile(JToken json, long teamID)
        {
            //The teamID needs to be passed in as a parameter since the Valve API for a team's info doesn't incldue its ID
            ID = teamID;
            Name = (string)json["name"];
            Tag = (string)json["tag"];
            LogoID = (long)json["logo"];
            CountryCode = (string)json["country_code"];
            List<long> playerIDs = new List<long>();
            int counter = 0;
            while (json[String.Format("player_{0}_account_id", counter)] != null)
            {
                playerIDs.Add((long)json[String.Format("player_{0}_account_id", counter)]);
                counter++;
            }
            PlayerIDs = playerIDs;
            CaptainID = (long)json["admin_account_id"];
            counter = 0;
            List<long> playedTournamentIDs = new List<long>();
            while (json[String.Format("league_id_{0}", counter)] != null)
            {
                playedTournamentIDs.Add((long)json[String.Format("league_id_{0}", counter)]);
                counter++;
            }
            PlayedTournamentIDs = playedTournamentIDs;
        }
    }
}

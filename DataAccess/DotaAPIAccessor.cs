using DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace DataAccess
{
    public class DotaAPIAccessor
    {
        //For information on how the Dota 2 API works visit https://wiki.teamfortress.com/wiki/WebAPI

        //TODO: LOAD THIS FROM SOMEWHERE. DON'T COMMIT TO GIT
        private string devKey = "";
        //This is the ID for Dota 2 in the Valve API
        private string id = "570";
        private string baseAddress = "http://api.steampowered.com/";

        public List<JsonTournament> GetAllTournaments()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetLeagueListing/v1?key={1}", id, devKey)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"].First().First();
                List<JsonTournament> tournamnets = new List<JsonTournament>();
                foreach (var tournament in json)
                {
                    tournamnets.Add(new JsonTournament(tournament));
                }
                return tournamnets;
            }
        }


        public List<JsonLiveMatch> GetAllLiveTournamentGames()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetLiveLeagueGames/v1?key={1}", id, devKey)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"].First().First();
                List<JsonLiveMatch> matches = new List<JsonLiveMatch>();
                foreach (var match in json)
                {
                    matches.Add(new JsonLiveMatch(match));
                }
                return matches;
            }
        }

        public JsonMatch GetMatch(long matchID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetMatchDetails/v1?key={1}&match_id={2}", id, devKey, matchID)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                return new JsonMatch(json);
            }
        }
    }
}

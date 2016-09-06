﻿using DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;
using System.Net;

namespace DataAccess
{
    public class DotaAPIAccessor
    {
        //For information on how the Dota 2 API works visit https://wiki.teamfortress.com/wiki/WebAPI
        private string devKey;
        private string gameID;
        private string baseAddress;
        private string languageCode;

        public DotaAPIAccessor()
        {
            devKey = ResourceLoader.GetForViewIndependentUse("DataAccess/APIKeys").GetString("Dota2APIKey");
            if (devKey == "")
            {
                throw new Exception("Add an API key to APIKeys.resw for the Dota 2 API. A key can be generated at: http://steamcommunity.com/dev/apikey");
            }
            gameID = "570";
            baseAddress = "http://api.steampowered.com/";
            //This can be changed to be modified in the future depending on the user's language. Default to english
            languageCode = "en";
        }

        public IEnumerable<JsonTournament> GetAllTournaments()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetLeagueListing/v1?key={1}", gameID, devKey)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"].First().First();
                List<JsonTournament> tournamnets = new List<JsonTournament>();
                foreach (var tournament in json)
                {
                    tournamnets.Add(new JsonTournament(tournament));
                }
                return tournamnets;
            }
        }


        public IEnumerable<JsonLiveMatch> GetAllLiveTournamentGames()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetLiveLeagueGames/v1?key={1}", gameID, devKey)).Result;
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
                bool success = false;
                HttpResponseMessage response = null;
                while (!success)
                {
                    response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetMatchDetails/v1?key={1}&match_id={2}", gameID, devKey, matchID)).Result;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            success = true;
                            break;
                        case HttpStatusCode.ServiceUnavailable:
                            //We may have been sending too many requests or the service is temporarily down. Try again
                            success = false;
                            break;
                        default:
                            //A new unseen response message occured - should check it and create better error handling
                            return null;
                    }
                }
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                return new JsonMatch(json);
            }
        }

        public IEnumerable<long> GetMatchIDsForTournament(long tournamentID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                //500 is the maximum number of games that can be requested
                //It takes an absurdly long time to process even 20 games. For now limiting to 5
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetMatchHistory/v1?key={1}&league_id={2}&matches_requested=5", gameID, devKey, tournamentID)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                json["matches"].OrderByDescending(t => (new TimeSpan((long)t["start_time"])));
                List<long> matches = new List<long>();
                foreach (var match in json["matches"])
                {
                    matches.Add((long)match["match_id"]);
                }
                return matches;
            }
        }

        public Uri GetImageURL(long imageID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("ISteamRemoteStorage/GetUGCFileDetails/v1?key={0}&appid={1}&ugcid={2}", devKey, gameID, imageID)).Result;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    //The team does not have a logo or the logo doesn't exist
                    return null;
                }
                else if (response.StatusCode != HttpStatusCode.OK)
                {
                    //Exception occured!
                    return null;
                }
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["data"];
                return new Uri((string)json["url"]);
            }
        }

        public JsonTeamProfile GetTeamInfo(long teamID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                bool success = false;
                HttpResponseMessage response = null;
                while (!success)
                {
                    response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetTeamInfoByTeamID/v1?key={1}&start_at_team_id={2}&teams_requested=1", gameID, devKey, teamID)).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        success = true;
                    }
                    //Keep trying to contact Valve's API in the event of a time out
                    else if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                    {
                        success = false;
                    }
                }

                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"]["teams"].FirstOrDefault();
                if (json == null)
                {
                    return null;
                }
                return new JsonTeamProfile(json, teamID);
            }
        }

        public IEnumerable<string> GetHeroes()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IEconDOTA2_{0}/GetHeroes/v1?key={1}&language={2}", gameID, devKey, languageCode)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                List<string> heroes = new List<string>();
                //A hero ID of 0 means the player has not yet selected a hero
                heroes.Add("None");
                heroes.AddRange(json["heroes"].OrderBy(t => (int)t["id"]).Select(t => (string)t["localized_name"]));
                return heroes;
            }
        }

        public IEnumerable<string> GetItems()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("IEconDOTA2_{0}/GetGameItems/v1?key={1}&language={2}", gameID, devKey, languageCode)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                List<string> items = new List<string>();
                //An item ID of 0 means the item slot is empty
                items.Add("Empty");
                items.AddRange(json["items"].OrderBy(t => (int)t["id"]).Select(t => (string)t["localized_name"]));
                return items;
            }
        }

        public JsonAccount GetAccountInfo(long accountID)
        {
            //Convert the accountID from 32 bit to 64 bit if necessary.
            if (accountID < 76561197960265728)
            {
                accountID += 76561197960265728;
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                try
                {
                    var response = client.GetAsync(string.Format("ISteamUser/GetPlayerSummaries/v0002?key={0}&steamids={1}", devKey, accountID)).Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //exception happened
                        return null;
                    }
                    JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["response"]["players"].First();
                    return new JsonAccount(json);
                }
                catch (Exception e)
                {
                    //TEsting to see if exception is occuring here
                    return null;
                }
                //var response = client.GetAsync(string.Format("ISteamUser/GetPlayerSummaries/v0002?key={0}&steamids={1}", devKey, accountID)).Result;
                //JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["response"]["players"].First();
                //return new JsonAccount(json);
            }
        }
    }
}

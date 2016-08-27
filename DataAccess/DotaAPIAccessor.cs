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

namespace DataAccess
{
    public class DotaAPIAccessor
    {
        //For information on how the Dota 2 API works visit https://wiki.teamfortress.com/wiki/WebAPI
        private string devKey;
        private string gameID;
        private string baseAddress;

        public DotaAPIAccessor()
        {
            devKey = ResourceLoader.GetForViewIndependentUse("DataAccess/APIKeys").GetString("Dota2APIKey");
            if (devKey == "")
            {
                throw new Exception("Add an API key to APIKeys.resw for the Dota 2 API. A key can be generated at: http://steamcommunity.com/dev/apikey");
            }
            gameID = "570";
            baseAddress = "http://api.steampowered.com/";
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
                var response = client.GetAsync(string.Format("IDOTA2Match_{0}/GetMatchDetails/v1?key={1}&match_id={2}", gameID, devKey, matchID)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
                return new JsonMatch(json);
            }
        }

        public Uri GetImageURL(long imageID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = client.GetAsync(string.Format("ISteamRemoteStorage/GetUGCFileDetails/v1?key={0}&appid={1}&ugcid={2}", devKey, gameID, imageID)).Result;
                JToken json = JObject.Parse(response.Content.ReadAsStringAsync().Result)["data"];
                return new Uri((string)json["url"]);
            }
        }

        //Liquid logo id: 451783905032671206
    }
}

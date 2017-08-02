using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using DataAccess;
using DataAccess.Models;
using System.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class DotaAPIAccessorTest
    {
        private DotaAPIAccessor accessor = new DotaAPIAccessor();

        [TestMethod]
        public void GetAllTournaments()
        {
            var tournaments = accessor.GetAllTournaments();
            CollectionAssert.AllItemsAreInstancesOfType(tournaments.ToList(), typeof(JsonTournament));
        }

        [TestMethod]
        public void GetAllLiveTournamentGames()
        {
            var liveGames = accessor.GetAllLiveTournamentGames();
            CollectionAssert.AllItemsAreInstancesOfType(liveGames.ToList(), typeof(JsonLiveMatch));
        }

        [DataTestMethod]
        [DataRow(2558534849, 4664)]
        public void GetMatch(long matchID, long leagueID)
        {
            var match = accessor.GetMatch(matchID);
            Assert.AreEqual(leagueID, match.LeagueID);
        }

        [DataTestMethod]
        [DataRow(4664, 217)]
        public void GetMatchIDsForTournament(long tournamentID, int numMatches)
        {
            var matches = accessor.GetMatchesForTournament(tournamentID, numMatches);
            Assert.AreEqual(numMatches, matches.Count());
        }

        [DataTestMethod]
        [DataRow(451783905032671206, "http://cloud-3.steamusercontent.com/ugc/451783905032671206/4CBD3B9B03D9515A20D471437EF0FEB81363C198/")]
        public void GetImageURL(long imageID, string uri)
        {
            var imageURL = accessor.GetImageURL(imageID);
            Assert.AreEqual(uri, imageURL.OriginalString);
        }

        [DataTestMethod]
        [DataRow(36, "Natus Vincere")]
        [DataRow(2586976, "OG Dota2")]
        [DataRow(1838315, "Team Secret")]
        public void GetTeamInfo(long teamID, string teamName)
        {
            var teamInfo = accessor.GetTeamInfo(teamID);
            Assert.AreEqual(teamName, teamInfo.Name);
        }

        [TestMethod]
        public void GetHeroes()
        {
            var heroes = accessor.GetHeroes();
            Assert.AreEqual("Anti-Mage", heroes.ElementAt(1));
        }

        [TestMethod]
        public void GetItems()
        {
            var items = accessor.GetItems();
            Assert.AreEqual("Blink Dagger", items.ElementAt(1));
        }

        [DataTestMethod]
        [DataRow(70388657, "Dendi")]
        [DataRow(76561197960265728 + 70388657, "Dendi")]
        public void GetAccountInfo(long accountID, string name)
        {
            var account = accessor.GetAccountInfo(accountID);
            Assert.IsTrue(account.URL.ToString().Contains("DendiQ"));
        }
    }
}

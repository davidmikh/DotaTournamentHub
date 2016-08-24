using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using DataAccess;
using DataAccess.Models;

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
            CollectionAssert.AllItemsAreInstancesOfType(tournaments, typeof(JsonTournament));
        }

        [TestMethod]
        public void GetAllLiveTournamentGames()
        {
            var liveGames = accessor.GetAllLiveTournamentGames();
            CollectionAssert.AllItemsAreInstancesOfType(liveGames, typeof(JsonLiveMatch));
        }

        [DataTestMethod]
        [DataRow(2558534849, 4664)]
        public void GetMatch(long matchID, long leagueID)
        {
            var match = accessor.GetMatch(matchID);
            Assert.AreEqual(match.LeagueID, leagueID);
        }
    }
}

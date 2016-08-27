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
        [DataRow(451783905032671206, "http://cloud-3.steamusercontent.com/ugc/451783905032671206/4CBD3B9B03D9515A20D471437EF0FEB81363C198/")]
        public void GetImageURL(long imageID, string uri)
        {
            var imageURL = accessor.GetImageURL(imageID);
            Assert.AreEqual(uri, imageURL.OriginalString);
        }
    }
}

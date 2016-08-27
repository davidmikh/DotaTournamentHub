using CoreDataModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public class TournamentManager
    {
        private DotaAPIAccessor accessor;

        public TournamentManager()
        {
            accessor = new DotaAPIAccessor();
        }

        //public IEnumerable<Match> GetLiveTournamentGames()
        //{
        //    var matches = accessor.GetAllLiveTournamentGames();
        //    //This filters out all unofficial teams in games like FACEIT Leagues
        //    matches = matches.Where(t => t.Radiant != null && t.Dire != null);
        //}
    }
}

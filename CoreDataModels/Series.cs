using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Series
    {
        public List<Match> Matches { get; set; }
        public DateTime Start { get; set; }
        public Team Winner
        {
            get
            {
                if (Matches.Where(t => t.Status.Equals(GameStatus.NotStarted) || t.Status.Equals(GameStatus.InProgress)).Count() != 0)
                {
                    //The series is not yet complete
                    return null;
                }

                Team teamA = Matches.First().Radiant;
                Team teamB = Matches.First().Dire;

                int teamAVictories = Matches.Where(t => (t.Status.Equals(GameStatus.RadiantVictory) && t.Radiant == teamA) || (t.Status.Equals(GameStatus.DireVictory) && t.Dire == teamA)).Count();
                int teamBVictories = Matches.Where(t => (t.Status.Equals(GameStatus.RadiantVictory) && t.Radiant == teamB) || (t.Status.Equals(GameStatus.DireVictory) && t.Dire == teamB)).Count();

                if (teamAVictories > teamBVictories)
                {
                    return teamA;
                }
                else if (teamAVictories < teamBVictories)
                {
                    return teamB;
                }
                else
                {
                    //This can occur in a group stage match where the series is a best of an even number.
                    throw new Exception("No winner for this series, it was a tie");
                }
            }
        }

        public Series(List<Match> matches, DateTime start)
        {
            Matches = matches;
            Start = start;
        }
    }
}

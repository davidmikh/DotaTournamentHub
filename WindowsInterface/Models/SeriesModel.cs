using CoreDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInterface.Models.Base;

namespace WindowsInterface.Models
{
    public class SeriesModel : EventModelBase
    {
        public OfficialTeam TeamA { get; set; }
        public OfficialTeam TeamB { get; set; }
        public string Matchup
        {
            get
            {
                return TeamA.Name + " vs " + TeamB.Name + " - " + GetStartTime();
            }
        }

        public SeriesModel(OfficialTeam teamA, OfficialTeam teamB, DateTime start)
        {
            TeamA = teamA;
            TeamB = teamB;
            Start = start;
        }
    }
}

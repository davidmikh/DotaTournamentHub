using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public abstract class Match
    {
        public long ID { get; set; }
        public long LeagueID { get; set; }
        public Team Radiant { get; set; }
        public Team Dire { get; set; }
        public int GameType { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

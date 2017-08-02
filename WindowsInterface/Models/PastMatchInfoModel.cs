using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInterface.Models
{
    public class PastMatchInfoModel
    {
        public long ID;
        public TournamentModel Tournament { get; set; }
        public GameTeamModel Radiant { get; set; }
        public GameTeamModel Dire { get; set; }
        public int GameType { get; set; }
        public TimeSpan Duration { get; set; }
        public bool RadiantVictory { get; set; }
        public DateTime StartTime { get; set; }

    }
}

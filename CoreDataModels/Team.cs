using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Team
    {
        public int Kills { get; set; }
        public string[] Picks { get; set; }
        public string[] Bans { get; set; }
        public int TowerState { get; set; }
        public int BarracksState { get; set; }
        public OfficialTeam OfficialTeam { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}

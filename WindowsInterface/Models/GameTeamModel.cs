using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInterface.Models
{
    public class GameTeamModel
    {
        public int Kills { get; set; }
        public string[] Bans { get; set; }
        public int TowerState { get; set; }
        public int BarracksState { get; set; }
        public OfficialTeamModel OfficialTeam { get; set; }
        public IEnumerable<GamePlayerModel> Players { get; set; }
    }
}

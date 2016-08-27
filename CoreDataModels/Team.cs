using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Team
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public string Region { get; set; }

        public Player Captain { get; set; }
        public List<Player> Players { get; set; }
        public List<Player> Substitutes { get; set; }

        public Team(string name, string region, Player captain, List<Player> players, List<Player> substitutes = null)
        {
            Name = name;
            Region = region;
            Captain = captain;
            Players = players;
            Substitutes = substitutes;

            if (Substitutes == null)
            {
                Substitutes = new List<Player>();
            }
        }
    }
}

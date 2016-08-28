using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class PastMatch : Match
    {
        public bool RadiantVictory { get; set; }
        public DateTime StartTime { get; set; }
        public int ServerClusterNum { get; set; }
        public bool IsSourceTwo { get; set; }
    }
}

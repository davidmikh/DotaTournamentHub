using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class LiveMatch : Match
    {
        public long SeriesID { get; set; }
        public int GameNumber { get; set; }
        public int RadiantSeriesWins { get; set; }
        public int DireSeriesWins { get; set; }
        public int SeriesType { get; set; }
    }
}

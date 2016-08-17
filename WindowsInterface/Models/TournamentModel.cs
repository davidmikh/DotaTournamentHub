using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInterface.Models.Base;

namespace WindowsInterface.Models
{
    public class TournamentModel : EventModelBase
    {
        public string Name { get; set; }
        public List<SeriesModel> Games { get; set; }
        public string NameAndStartTime
        {
            get
            {
                return Name + " - " + GetStartTime();
            }
        }

        public TournamentModel(string name, List<SeriesModel> games, DateTime start)
        {
            Name = name;
            Games = games;
            Start = start;
        }
    }
}

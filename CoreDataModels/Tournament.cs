using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Tournament
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<Series> GroupStage { get; set; }
        public List<Series> MainEvent { get; set; }

        public Tournament(string name, DateTime start, DateTime end, List<Series> groupStage = null, List<Series> mainEvent = null)
        {
            Name = name;
            Start = start;
            End = end;

            GroupStage = groupStage;
            MainEvent = mainEvent;
        }
    }
}

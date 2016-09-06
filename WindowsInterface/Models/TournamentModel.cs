using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInterface.Models
{
    public class TournamentModel
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public ObservableCollection<MatchModel> Matches { get; set; }
    }
}

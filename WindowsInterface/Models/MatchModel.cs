using CoreDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInterface.Models
{
    public class MatchModel
    {
        public long ID { get; set; }
        public bool IsLive { get; set; }
        public TeamModel Radiant { get; set; }
        public TeamModel Dire { get; set; }
    }
}

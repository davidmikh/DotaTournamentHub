using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class OfficialTeam
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public long CaptainID { get; set; }
        public IEnumerable<ProAccount> Players { get; set; }
        public Uri LogoURL { get; set; }
    }
}

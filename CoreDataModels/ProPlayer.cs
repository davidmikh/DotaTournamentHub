using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class ProPlayer
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Uri URL { get; set; }
        public Uri Image { get; set; }
        public string RealName { get; set; }
        public string CountryCode { get; set; }
    }
}

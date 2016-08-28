using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class ProPlayer
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public string RealName { get; set; }
        public string Position { get; set; }

        public ProPlayer(string name, long id, string realName, string position)
        {
            Name = name;
            ID = id;
            RealName = realName;
            Position = position;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Player
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Position { get; set; }

        public Player(string name, string realName, string position)
        {
            Name = name;
            RealName = realName;
            Position = position;
        }
    }
}

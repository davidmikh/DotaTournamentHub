using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Player
    {
        public ProPlayer ProPlayer { get; set; }
        public string Hero { get; set; }
        public int Slot { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int LastHits { get; set; }
        public int Denies { get; set; }
        public int CurrentGold { get; set; }
        public int Level { get; set; }
        public int GPM { get; set; }
        public int XPM { get; set; }
        public int UltimateState { get; set; }
        public int UltimateCD { get; set; }
        public string[] Items { get; set; }
        public int RespawnTime { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public int NetWorth { get; set; }
    }
}

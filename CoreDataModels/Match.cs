using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataModels
{
    public class Match
    {
        public int ID { get; set; }
        public Team Radiant { get; set; }
        public Team Dire { get; set; }
        public GameStatus Status { get; set; }

        public Match(int id, Team radiant, Team dire, GameStatus status)
        {
            ID = id;
            Radiant = radiant;
            Dire = dire;
            Status = status;
        }
    }

    public enum GameStatus
    {
        NotStarted,
        InProgress,
        RadiantVictory,
        DireVictory
    }
}

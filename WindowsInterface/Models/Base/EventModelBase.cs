using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsInterface.Models.Base
{
    public abstract class EventModelBase
    {
        public DateTime Start { get; set; }

        public string GetStartTime()
        {
            TimeSpan remainingTime = Start - DateTime.Now;
            StringBuilder time = new StringBuilder();
            if (remainingTime.Days > 0)
            {
                time.Append(remainingTime.Days + "d ");
            }
            time.Append(remainingTime.Hours + "h " + remainingTime.Minutes + "m");
            return time.ToString();
        }
    }
}

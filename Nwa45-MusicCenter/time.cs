using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwa45_MusicCenter
{
    class time
    {
        public int day;
        public int hour;
        public int minute;
        public int second;

        public time(int d, int h, int m, int s)
        {
            this.day = d;
            this.hour = h;
            this.minute = m;
            this.second = s;
        }

        public void show()
        {
            Console.WriteLine(String.Format("{0}:{1}:{2}:{3}", this.day, this.hour, this.minute, this.second));
        }

        public double getsecond()
        {
            int Day = this.day;
            int Hour = this.hour;
            int Minute = this.minute;
            int Second = this.second;


            Hour += Day * 24;
            Minute += Hour * 60;
            Second += Minute * 60;

            return Second;
        }
    }
}

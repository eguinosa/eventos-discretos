using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class Hour
    {
        public static bool HoraPico(decimal minutes)
        {
            if (minutes > 90 && minutes < 210) return true;
            if (minutes > 420 && minutes < 540) return true;
            return false;
        }

        public static (int hours, int minutes) Transform(decimal minutes)
        {
            var result = Transform((int)Math.Round(minutes));
            return result;
        }

        public static (int hours, int minutes) Transform(int minutes)
        {
            int h = minutes / 60;
            int m = minutes % 60;
            return (h, m);
        }

        public static string HourToString((int hours, int minutes) x)
        {
            var temp = x.hours % 12;
            var h = temp >= 10 ? temp.ToString() : "0" + temp;
            h = temp == 0 ? "12" : h;
            var m = x.minutes >= 10 ? x.minutes.ToString() : "0" + x.minutes;
            var t = (x.hours % 24) < 12 ? "am" : "pm";

            return h + ":" + m + t;
        }
    }
}

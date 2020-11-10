using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class Function
    {
        protected decimal m;
        protected decimal n;

        public Function((decimal x, decimal y) a, (decimal x, decimal y) b)
        {
            if (a.x == b.x)
            {
                m = 0;
                n = a.y;
            }
            else
            {
                m = (a.y - b.y) / (a.x - b.x);
                n = a.y - m * a.x;
            }
        }

        public decimal F(decimal x)
        {
            decimal result = m * x + n;
            return result;
        }
    }
}

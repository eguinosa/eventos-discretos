using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class MultiFunction
    {
        protected decimal minValueX;
        protected decimal maxValueX;

        public decimal MaxValueF { get; protected set; }

        protected List<Function> functions;
        protected List<(decimal x, decimal y)> points;

        public MultiFunction(params (decimal x, decimal y)[] ps)
        {
            if (ps.Length < 2) throw new Exception("2 or more points are required to create a Function");

            minValueX = ps[0].x;
            maxValueX = ps[ps.Length - 1].x;
            MaxValueF = ps[0].y;

            functions = new List<Function>();
            points = new List<(decimal x, decimal y)> { ps[0] };

            for (int i = 1; i < ps.Length; i++)
            {
                if (MaxValueF < ps[i].y) MaxValueF = ps[i].y;

                var a = ps[i - 1];
                var b = ps[i];

                if (a.x == b.x && a.y == b.y) throw new Exception("Points need to be different to create a Function");
                if (a.x > b.x) throw new Exception("X values most be sorted");

                points.Add(b);
                var f = new Function(a, b);
                functions.Add(f);
            }
        }

        public decimal F(decimal x)
        {
            if (x < minValueX || x > maxValueX) return 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (x >= points[i].x && x <= points[i + 1].x)
                {
                    var result = functions[i].F(x);
                    return result;
                }
            }
            return 0;
        }
    }
}

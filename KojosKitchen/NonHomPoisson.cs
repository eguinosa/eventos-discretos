using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class NonHomPoisson
    {
        protected Random r;

        public NonHomPoisson()
        {
            r = new Random(DateTime.Now.Millisecond);
        }

        public List<decimal> Process(decimal maxLambda, MultiFunction fLambda, decimal time)
        {
            decimal t = 0;
            var result = new List<decimal>();

            while (t < time)
            {
                var u = r.NextDouble();
                t -= (1 / maxLambda) * (decimal)Math.Log(u);
                if (t > time) break;

                u = r.NextDouble();
                if ((decimal)u <= fLambda.F(t) / maxLambda) result.Add(t);
            }
            return result;
        }
    }
}

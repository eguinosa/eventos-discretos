using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    enum Kojos
    {
        Sandwich,
        Sushi
    }

    enum Worker
    {
        Worker1,
        Worker2,
        Worker3
    }

    class Extra
    {
        public static List<(decimal time, Kojos type)> Arrivals(List<decimal> sandArrivals, List<decimal> sushArrivals)
        {
            int i = 0, j = 0;
            var result = new List<(decimal, Kojos)>();

            while(i < sandArrivals.Count && j < sushArrivals.Count)
            {
                if(sandArrivals[i] <= sushArrivals[j])
                {
                    result.Add((sandArrivals[i], Kojos.Sandwich));
                    i++;
                }
                else
                {
                    result.Add((sushArrivals[j], Kojos.Sushi));
                    j++;
                }
            }
            while(i < sandArrivals.Count)
            {
                result.Add((sandArrivals[i], Kojos.Sandwich));
                i++;
            }
            while(j < sushArrivals.Count)
            {
                result.Add((sushArrivals[j], Kojos.Sushi));
                j++;
            }

            return result;
        }        
    }
}

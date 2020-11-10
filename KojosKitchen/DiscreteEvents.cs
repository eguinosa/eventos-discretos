using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class DiscreteEvents
    {
        protected Random r;

        public DiscreteEvents()
        {
            r = new Random();
        }

        public List<(decimal arrival, decimal service, decimal departure, Kojos type, Worker worker)> TwoWorkers(List<(decimal, Kojos)> arrivals, decimal totalTime)
        {
            //Initialization:
            decimal time;
            var line = new Queue<(decimal time, Kojos type)>(arrivals);

            var ta = line.Count > 0 ? line.Peek().time : decimal.MaxValue;
            var t1 = decimal.MaxValue;
            var t2 = decimal.MaxValue;

            int worker1 = -1, worker2 = -1;
            var waiting = new Queue<(int id, Kojos type)>();
            var result = new List<(decimal arrival, decimal service, decimal departure, Kojos type, Worker worker)>();

            while (ta <= totalTime || worker1 != -1 || worker2 != -1)
            {
                if (ta <= Math.Min(t1, t2) && ta <= totalTime)
                {
                    time = ta;
                    var client = line.Dequeue();
                    ta = line.Count > 0 ? line.Peek().time : decimal.MaxValue;

                    if (worker1 == -1)
                    {
                        result.Add((time, time, -1, client.type, Worker.Worker1));
                        worker1 = result.Count - 1;
                        t1 = time + RandomUniform(MinWorkTime(client.type), MaxWorkTime(client.type));
                    }
                    else if (worker2 == -1)
                    {
                        result.Add((time, time, -1, client.type, Worker.Worker2));
                        worker2 = result.Count - 1;
                        t2 = time + RandomUniform(MinWorkTime(client.type), MaxWorkTime(client.type));
                    }
                    else
                    {
                        result.Add((time, -1, -1, client.type, 0));
                        waiting.Enqueue((result.Count - 1, client.type));
                    }
                }
                if (t1 <= Math.Min(ta, t2) && worker1 != -1)
                {
                    time = t1;
                    result[worker1] = (result[worker1].arrival, result[worker1].service, time, result[worker1].type, Worker.Worker1);

                    if (waiting.Count > 0)
                    {
                        var (id, type) = waiting.Dequeue();
                        result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker1);
                        worker1 = id;
                        t1 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                    }
                    else
                    {
                        worker1 = -1;
                        t1 = decimal.MaxValue;
                    }
                }
                if (t2 <= Math.Min(ta, t1) && worker2 != -1)
                {
                    time = t2;
                    result[worker2] = (result[worker2].arrival, result[worker2].service, time, result[worker2].type, Worker.Worker2);

                    if (waiting.Count > 0)
                    {
                        var (id, type) = waiting.Dequeue();
                        result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker2);
                        worker2 = id;
                        t2 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                    }
                    else
                    {
                        worker2 = -1;
                        t2 = decimal.MaxValue;
                    }
                }
            }

            return result;
        }

        public List<(decimal arrival, decimal service, decimal departure, Kojos type, Worker worker)> ThreeWorkers(List<(decimal, Kojos)> arrivals, decimal totalTime)
        {
            //Initialization:
            decimal time = 0;
            var line = new Queue<(decimal time, Kojos type)>(arrivals);

            var ta = line.Count > 0 ? line.Peek().time : decimal.MaxValue;
            var t1 = decimal.MaxValue;
            var t2 = decimal.MaxValue;
            var t3 = decimal.MaxValue;

            var worker1 = -1;
            var worker2 = -1;
            var worker3 = -1;
            var waiting = new Queue<(int id, Kojos type)>();
            var result = new List<(decimal arrival, decimal service, decimal departure, Kojos type, Worker worker)>();

            while (ta <= totalTime || worker1 != -1 || worker2 != -1 || worker3 != -1)
            {
                if (ta <= Math.Min(t1, Math.Min(t2, t3)) && ta <= totalTime)
                {
                    time = ta;
                    var client = line.Dequeue();
                    ta = line.Count > 0 ? line.Peek().time : decimal.MaxValue;

                    if (worker1 == -1)
                    {
                        result.Add((time, time, -1, client.type, Worker.Worker1));
                        worker1 = result.Count - 1;
                        t1 = time + RandomUniform(MinWorkTime(client.type), MaxWorkTime(client.type));
                    }
                    else if (worker2 == -1)
                    {
                        result.Add((time, time, -1, client.type, Worker.Worker2));
                        worker2 = result.Count - 1;
                        t2 = time + RandomUniform(MinWorkTime(client.type), MaxWorkTime(client.type));
                    }
                    else if (worker3 == -1 && Hour.HoraPico(time))
                    {
                        result.Add((time, time, -1, client.type, Worker.Worker3));
                        worker3 = result.Count - 1;
                        t3 = time + RandomUniform(MinWorkTime(client.type), MaxWorkTime(client.type));
                    }
                    else
                    {
                        result.Add((time, -1, -1, client.type, 0));
                        waiting.Enqueue((result.Count - 1, client.type));
                    }
                }
                if (t1 <= Math.Min(ta, Math.Min(t2, t3)) && worker1 != -1)
                {
                    time = t1;
                    result[worker1] = (result[worker1].arrival, result[worker1].service, time, result[worker1].type, Worker.Worker1);

                    if (waiting.Count > 0)
                    {
                        var (id, type) = waiting.Dequeue();
                        result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker1);
                        worker1 = id;
                        t1 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                    }
                    else
                    {
                        worker1 = -1;
                        t1 = decimal.MaxValue;
                    }
                }
                if (t2 <= Math.Min(ta, Math.Min(t1, t3)) && worker2 != -1)
                {
                    time = t2;
                    result[worker2] = (result[worker2].arrival, result[worker2].service, time, result[worker2].type, Worker.Worker2);

                    if (waiting.Count > 0)
                    {
                        var (id, type) = waiting.Dequeue();
                        result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker2);
                        worker2 = id;
                        t2 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                    }
                    else
                    {
                        worker2 = -1;
                        t2 = decimal.MaxValue;
                    }
                }
                if (t3 <= Math.Min(ta, Math.Min(t1, t2)) && worker3 != -1)
                {
                    time = t3;
                    result[worker3] = (result[worker3].arrival, result[worker3].service, time, result[worker3].type, Worker.Worker3);

                    if (waiting.Count > 0 && Hour.HoraPico(time))
                    {
                        var (id, type) = waiting.Dequeue();
                        result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker3);
                        worker3 = id;
                        t3 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                    }
                    else
                    {
                        worker3 = -1;
                        t3 = decimal.MaxValue;
                    }
                }
                if (worker3 == -1 && Hour.HoraPico(time) && waiting.Count > 0)
                {
                    var (id, type) = waiting.Dequeue();
                    result[id] = (result[id].arrival, time, -1, result[id].type, Worker.Worker3);
                    worker3 = id;
                    t3 = time + RandomUniform(MinWorkTime(type), MaxWorkTime(type));
                }
            }

            return result;
        }

        private decimal RandomUniform(decimal a, decimal b)
        {
            var u = (decimal)r.NextDouble();
            var result = a + (b - a) * u;
            return result;
        }

        private decimal MinWorkTime(Kojos type)
        {
            if (type == Kojos.Sandwich) return 3;
            else return 5;
        }

        private decimal MaxWorkTime(Kojos type)
        {
            if (type == Kojos.Sandwich) return 5;
            else return 8;
        }
    }
}

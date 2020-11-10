using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KojosKitchen
{
    class Program
    {
        static void Main(string[] args)
        {
            //Informacion Inicial
            Console.WriteLine("Programa para simular el tiempo de espera de los consumidores que visitan 'La Cocina de Kojo'");
            Console.WriteLine("Vende 2 tipos de productos: Sandwiches y Sushi.");
            Console.WriteLine("Trabaja desde las 10:00am hasta las 09:00pm con dos horarios pico:");
            Console.WriteLine("11:30am - 01:30pm\n05:00pm - 07:00pm\n");
            while (true)
            {
                //Pidiendo los datos necesarios para realizar la simulacion
                Console.Write("Intruduzca el promedio de Sandwiches vendidos diariamente: ");
                var sandwich = decimal.Parse(Console.ReadLine());
                Console.Write("Intruduzca el promedio de Sushi vendidos diariamente: ");
                var sushi = decimal.Parse(Console.ReadLine());
                Console.Write("El promedio de Sandwiches vendidos en el 1er Horario Pico: ");
                var pSandwich1 = decimal.Parse(Console.ReadLine());
                Console.Write("El promedio de Sushi vendidos en el 1er Horario Pico: ");
                var pSushi1 = decimal.Parse(Console.ReadLine());
                Console.Write("El promedio de Sandwiches vendidos en el 2do Horario Pico: ");
                var pSandwich2 = decimal.Parse(Console.ReadLine());
                Console.Write("El promedio de Sushi vendidos en el 2do Horario Pico: ");
                var pSushi2 = decimal.Parse(Console.ReadLine());
                //Controlando Errores en la entrada de valores:
                if(sandwich < pSandwich1 + pSandwich2)
                {
                    Console.WriteLine("\n El valor total de Sandwiches vendidos debe ser mayor que los vendidos en otros horarios\n");
                    continue;
                }
                if(sushi < pSushi1 + pSushi2)
                {
                    Console.WriteLine("\n El valor total de Sushi vendidos debe ser mayor que los vendidos en otros horarios\n");
                    continue;
                }

                //Productos vendidos fuera de Horario Pico:
                var oSandwich = sandwich - pSandwich1 - pSandwich2;
                var oSushi = sushi - pSushi1 - pSushi2;

                //Creando las variables de Tiempo (minutos):
                decimal peakTime = 2 * 60;
                decimal nonPeak = 7 * 60;
                decimal startPeak1 = 90;
                decimal startPeak2 = 7 * 60;
                decimal totalTime = 11 * 60;

                //Calculando los lambdas:
                var loutSandwich = oSandwich / nonPeak;
                var loutSushi = oSushi / nonPeak;
                var lpeakSandwich1 = pSandwich1 / peakTime;
                var lpeakSandwich2 = pSandwich2 / peakTime;
                var lpeakSushi1 = pSushi1 / peakTime;
                var lpeakSushi2 = pSushi2 / peakTime;

                //Creando los puntos (time, lambda) de la funcion lambda para el Poisson No Homogeneo:
                var startSandwich = (0, loutSandwich);
                var endSandwich = (totalTime, loutSandwich);
                var startSushi = (0, loutSushi);
                var endSushi = (totalTime, loutSushi);

                var startSandPeak1 = (startPeak1, loutSandwich);
                var sandwichPeak1 = (startPeak1 + 60, 2 * lpeakSandwich1 - loutSandwich);
                var endSandPeak1 = (startPeak1 + 120, loutSandwich);
                var startSushPeak1 = (startPeak1, loutSushi);
                var sushiPeak1 = (startPeak1 + 60, 2 * lpeakSushi1 - loutSushi);
                var endSushPeak1 = (startPeak1 + 120, loutSushi);

                var startSandPeak2 = (startPeak2, loutSandwich);
                var sandwichPeak2 = (startPeak2 + 60, 2 * lpeakSandwich2 - loutSandwich);
                var endSandPeak2 = (startPeak2 + 120, loutSandwich);
                var startSushPeak2 = (startPeak2, loutSushi);
                var sushiPeak2 = (startPeak2 + 60, 2 * lpeakSushi2 - loutSushi);
                var endSushPeak2 = (startPeak2 + 120, loutSushi);

                //Creando las Funciones Lambda:
                var funcSandwich = new MultiFunction(startSandwich, startSandPeak1, sandwichPeak1, endSandPeak1, startSandPeak2, sandwichPeak2, endSandPeak2, endSandwich);
                var funcSushi = new MultiFunction(startSushi, startSushPeak1, sushiPeak1, endSushPeak1, startSushPeak2, sushiPeak2, endSushPeak2, endSushi);

                //Generando los tiempo de llegadas de los consumidores:
                var poisson = new NonHomPoisson();
                var csandwich = poisson.Process(funcSandwich.MaxValueF, funcSandwich, totalTime);
                var csushi = poisson.Process(funcSushi.MaxValueF, funcSushi, totalTime);
                var arrivals = Extra.Arrivals(csandwich, csushi);
                Console.WriteLine("\n***** {0} clientes visitaron La Cocina de Kojo *****", arrivals.Count);
                Console.WriteLine("Sandwich -> {0}", csandwich.Count);
                Console.WriteLine("Sushi  ->  {0}", csushi.Count);

                //Analizando el tiempo de atencion con 2 empleados
                var events = new DiscreteEvents();
                var twoWorkers = events.TwoWorkers(arrivals, totalTime);
                int longWait2 = 0;
                Console.WriteLine("\n--- Trabajando con 2 empleados ---");
                Console.WriteLine("Cliente ###: <llegada> -> <servicio> -> <partida> [trabajador] [producto]");
                for (int i = 0; i < twoWorkers.Count; i++)
                {
                    if (twoWorkers[i].service - twoWorkers[i].arrival > 5) longWait2++;
                    var j = i + 1;
                    var num = j < 10 ? "00" + j : (j < 100 ? "0" + j : j.ToString());
                    var arr = Hour.HourToString(Hour.Transform(twoWorkers[i].arrival + 10 * 60));
                    var ser = Hour.HourToString(Hour.Transform(twoWorkers[i].service + 10 * 60));
                    var dep = Hour.HourToString(Hour.Transform(twoWorkers[i].departure + 10 * 60));
                    Console.WriteLine("Cliente {0}: <{1}> -> <{2}> -> <{3}> [{4}] [{5}]", num, arr, ser, dep, twoWorkers[i].worker, twoWorkers[i].type);
                }
                Console.WriteLine("Esperaron en la cola mas de 5 min, {0} clientes, el {1}%\n", longWait2, longWait2 * 100 / twoWorkers.Count);

                //Analizando el tiempo de atencion con 3er empleado en Horarios Picos:
                var threeWorkers = events.ThreeWorkers(arrivals, totalTime);
                int longWait3 = 0;
                Console.WriteLine("--- Trabajando con 3er empleado en horas Pico ---");
                Console.WriteLine("Cliente ###: <llegada> -> <servicio> -> <partida> [trabajador] [producto]");
                for (int i = 0; i < threeWorkers.Count; i++)
                {
                    if (threeWorkers[i].service - threeWorkers[i].arrival > 5) longWait3++;
                    var j = i + 1;
                    var num = j < 10 ? "00" + j : (j < 100 ? "0" + j : j.ToString());
                    var arr = Hour.HourToString(Hour.Transform(threeWorkers[i].arrival + 10 * 60));
                    var ser = Hour.HourToString(Hour.Transform(threeWorkers[i].service + 10 * 60));
                    var dep = Hour.HourToString(Hour.Transform(threeWorkers[i].departure + 10 * 60));
                    Console.WriteLine("Cliente {0}: <{1}> -> <{2}> -> <{3}> [{4}] [{5}]", num, arr, ser, dep, threeWorkers[i].worker, threeWorkers[i].type);
                }
                Console.WriteLine("Esperaron en la cola mas de 5 min, {0} clientes, el {1}%\n", longWait3, longWait3 * 100 / threeWorkers.Count);

                Console.WriteLine("Con 2 empleados: {0} clientes, el {1}%\n", longWait2, longWait2 * 100 / twoWorkers.Count);
                Console.WriteLine("Con 3 empleados: {0} clientes, el {1}%\n", longWait3, longWait3 * 100 / threeWorkers.Count);

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    class Program
    {
        static void Main(string[] args)
        {
            PercolationSimulation simul = new PercolationSimulation();
            PclData data = simul.MeanPercolationValue(200, 100);

            Console.WriteLine($"Moyenne : {data.Mean}");
            Console.WriteLine($"Ecart-type : {data.StandardDeviation}");

            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

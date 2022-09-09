using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public struct PclData
    {
        /// <summary>
        /// Moyenne 
        /// </summary>
        public double Mean { get; set; }
        /// <summary>
        /// Ecart-type
        /// </summary>
        public double StandardDeviation { get; set; }
        /// <summary>
        /// Fraction
        /// </summary>
        public double Fraction { get; set; }
    }

    public class PercolationSimulation
    {
        public PclData MeanPercolationValue(int size, int t)
        {
            PclData data = new PclData();
            double[] values = new double[t];
            double total = 0;
            
            for (int i = 0; i < t; i++)
            {
                values[i] = PercolationValue(size);
                total += values[i];
            }

            data.Mean = total / t;

            double somme = 0;

            foreach (double value in values)
            {
                somme += ((data.Mean - value) * (data.Mean - value));
            }

            data.StandardDeviation = Math.Sqrt(somme / t);

            return (data);
        }

        public double PercolationValue(int size)
        {
            Percolation perc = new Percolation(size);
            int cases_ouvertes = 0;
            Random rand = new Random();

            do
            {
                int x = rand.Next(size);
                int y = rand.Next(size);

                if (!perc.IsOpen(x, y))
                {
                    perc.Open(x, y);
                    cases_ouvertes += 1;
                }
            } while (!perc.Percolate());

            return ((double)cases_ouvertes / (size * size));
        }
    }
}

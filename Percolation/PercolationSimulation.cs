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
            
            // on lance la simulation t fois et on récupère à chaque fois la proportion cases ouvertes / nombre de cases.
            for (int i = 0; i < t; i++)
            {
                values[i] = PercolationValue(size);
                total += values[i];
            }

            // calcul de la moyenne des proportions.
            data.Mean = total / t;

            double somme = 0;

            // calcul de l'écart-type des proportions.
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

            // tant qu'il n'y a pas percolation...
            do
            {
                int x = rand.Next(size);
                int y = rand.Next(size);

                // on ouvre une case aléatoire si elle n'est pas déjà ouverte.
                if (!perc.IsOpen(x, y))
                {
                    perc.Open(x, y);
                    cases_ouvertes += 1;
                }
            } while (!perc.Percolate());

            // on retourne la valeur cases ouvertes / nombre de cases. 
            return ((double)cases_ouvertes / (size * size));
        }
    }
}

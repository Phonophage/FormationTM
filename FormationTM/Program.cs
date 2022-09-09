using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationTM
{
    class Program
    {
        static void Main(string[] args)
        {
            SchoolMeans("Input.csv", "Output.csv");
        }

        static string[] Moyennes (List<string> entrees)
        {
            double histTotal = 0;
            double mathTotal = 0;
            int nHist = 0;
            int nMath = 0;
            
            foreach (string ligne in entrees)
            {
                string matiere = ligne.Split(';')[1];
                string note = ligne.Split(';')[2];
                note = note.Replace('.', ',');

                if (matiere == "Histoire")
                {
                    histTotal += double.Parse(note);
                    nHist += 1;
                }
                else if (matiere == "Maths")
                {
                    mathTotal += double.Parse(note);
                    nMath += 1;
                }
            }

            double histMoyenne = histTotal / nHist;
            double mathMoyenne = mathTotal / nMath;
            string[] sorties = new string[2];
            sorties[0] = "Histoire;" + histMoyenne.ToString();
            sorties[1] = "Maths;" + mathMoyenne.ToString();

            return (sorties);
        }

        static void SchoolMeans(string input, string output)
        {
            using (FileStream inputStream = File.OpenRead(input))
            {
                using (TextReader lecteur = new StreamReader(inputStream))
                {
                    List<string> entrees = new List<string>();

                    while (lecteur.Peek() != -1)
                    {
                        entrees.Add(lecteur.ReadLine());
                    }
                    
                    string [] sorties = Moyennes(entrees);

                    using (FileStream outputStream = File.OpenWrite(output))
                    {
                        using (TextWriter ecrivain = new StreamWriter(outputStream))
                        {
                            foreach (string ligne in sorties)
                            {
                                ecrivain.WriteLine(ligne);
                            }
                        }
                    }
                }
            }
        }
    }
}

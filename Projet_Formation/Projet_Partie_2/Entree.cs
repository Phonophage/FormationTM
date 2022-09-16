using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Entree
    {
        public static List<Gestionnaire> LireFichierGest(string path)
        {
            List<string> dataGest = new List<string>();

            using (FileStream input = File.OpenRead(path))
            {
                using (StreamReader lecteur = new StreamReader(input))
                {
                    while (!lecteur.EndOfStream)
                    {
                        dataGest.Add(lecteur.ReadLine());
                    }
                }
            }

            return ParseDataGest(dataGest);
        }

        public static List<Gestionnaire> ParseDataGest(List<string> dataGest)
        {
            List<Gestionnaire> gestionnaires = new List<Gestionnaire>();

            foreach (string ligne in dataGest)
            {
                string[] data = ligne.Split(';');

                if (data.Length == 3) // 3 champs renseignés : identifiant, type, nombre transactions
                {
                    int identifiant = int.Parse(data[0]);

                    if (!Outils.GestExiste(gestionnaires, identifiant))
                    {
                        string type = data[1];
                        int nombreTransac = int.Parse(data[2]);

                        if (type == "Entreprise")
                        {
                            gestionnaires.Add(new Entreprise(identifiant, nombreTransac));
                        }
                        else if (type == "Particulier")
                        {
                            gestionnaires.Add(new Particulier(identifiant, nombreTransac));
                        }
                    }
                }
            }

            return gestionnaires;
        }

        public static List<Operation> LireFichierComptes(List<Gestionnaire> gestionnaires, string path)
        {
            List<string> dataComptes = new List<string>();

            using (FileStream input = File.OpenRead(path))
            {
                using (StreamReader lecteur = new StreamReader(input))
                {
                    while (!lecteur.EndOfStream)
                    {
                        dataComptes.Add(lecteur.ReadLine());
                    }
                }
            }

            return ParseDataComptes(gestionnaires, dataComptes);
        }

        public static List<Operation> ParseDataComptes(List<Gestionnaire> gestionnaires, List<string> dataComptes)
        {
            List<Operation> operations = new List<Operation>();

            foreach (string ligne in dataComptes)
            {
                string[] data = ligne.Split(';');

                if (data.Length == 5) // 5 champs renseignés : identifiant, date, solde, entrée, sortie
                {
                    int identifiant = int.Parse(data[0]);
                    DateTime date = DateTime.Parse(data[1]);
                    double solde = 0;

                    if (data[2].Length != 0)
                    {
                        solde = double.Parse(data[2]);
                    }

                    int entree = -1;
                    int sortie = -1;

                    if (data[3].Length != 0)
                    {
                        entree = int.Parse(data[3]);
                    }
                    if (data[4].Length != 0)
                    {
                        sortie = int.Parse(data[4]);
                    }
                    operations.Add(new Operation(identifiant, date, solde, entree, sortie));
                }
            }

            return operations;
        }

        public static List<Transaction> LireFichierTransactions(string path)
        {
            List<string> dataTransactions = new List<string>();

            using (FileStream input = File.OpenRead(path))
            {
                using (StreamReader lecteur = new StreamReader(input))
                {
                    while (!lecteur.EndOfStream)
                    {
                        dataTransactions.Add(lecteur.ReadLine());
                    }
                }
            }

            return ParseDataTransactions(dataTransactions);
        }

        public static List<Transaction> ParseDataTransactions(List<string> dataTransactions)
        {
            List<Transaction> transactions = new List<Transaction>();

            foreach (string ligne in dataTransactions)
            {
                string[] data = ligne.Split(';');

                if (data.Length == 5) // 5 champs renseignés : identifiant, date, montant, expéditeur, destinataire
                {
                    int identifiant = int.Parse(data[0]);
                    DateTime date = DateTime.Parse(data[1]);
                    double montant = double.Parse(data[2]);
                    int expediteur = int.Parse(data[3]);
                    int destinataire = int.Parse(data[4]);
                    bool doublon = Outils.TransactionExiste(transactions, identifiant);

                    transactions.Add(new Transaction(identifiant, date, expediteur, destinataire, montant, doublon));
                }
            }

            return transactions;
        }
    }
}

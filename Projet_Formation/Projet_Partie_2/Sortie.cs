using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Sortie
    {
        public static void EcrireSortieOperations(List<Operation> operations, string path)
        {
            using (FileStream output = File.OpenWrite(path))
            {
                using (StreamWriter ecrivain = new StreamWriter(output))
                {
                    foreach (Operation ope in operations)
                    {
                        if (ope.GetStatut())
                        {
                            ecrivain.Write($"{ope.GetIdentifiant()};{ope.GetDate():d};{ope.GetSolde()};");
                            if (ope.GetEntree() != -1)
                            {
                                ecrivain.Write(ope.GetEntree());
                            }
                            ecrivain.Write(";");
                            if (ope.GetSortie() != -1)
                            {
                                ecrivain.Write(ope.GetSortie());
                            }
                            ecrivain.WriteLine(";OK");
                        }
                        else
                        {
                            ecrivain.Write($"{ope.GetIdentifiant()};{ope.GetDate():d};{ope.GetSolde()};");
                            if (ope.GetEntree() != -1)
                            {
                                ecrivain.Write(ope.GetEntree());
                            }
                            ecrivain.Write(";");
                            if (ope.GetSortie() != -1)
                            {
                                ecrivain.Write(ope.GetSortie());
                            }
                            ecrivain.WriteLine(";KO");
                        }
                    }
                }
            }
        }

        public static void EcrireSortieTransactions(List<Transaction> transactions, string path)
        {
            using (FileStream output = File.OpenWrite(path))
            {
                using (StreamWriter ecrivain = new StreamWriter(output))
                {
                    foreach (Transaction tran in transactions)
                    {
                        if (tran.GetStatut())
                        {
                            ecrivain.WriteLine($"{tran.GetIdentifiant()};{tran.GetDate():d};{tran.GetMontant()};{tran.GetExpediteur()};{tran.GetDestinataire()};OK");
                        }
                        else
                        {
                            ecrivain.WriteLine($"{tran.GetIdentifiant()};{tran.GetDate():d};{tran.GetMontant()};{tran.GetExpediteur()};{tran.GetDestinataire()};KO");
                        }
                    }
                }
            }
        }

        public static void EcrireSortieMetrologie(List<Gestionnaire> gestionnaires, string path)
        {
            using (FileStream output = File.OpenWrite(path))
            {
                using (StreamWriter ecrivain = new StreamWriter(output))
                {
                    ecrivain.WriteLine("Statistiques :");
                    ecrivain.WriteLine($"Nombre de comptes : {Compte.GetNombreComptes()}");
                    ecrivain.WriteLine($"Nombre de transactions : {Transaction.GetNombreTransactions()}");
                    ecrivain.WriteLine($"Nombre de réussites : {Transaction.GetNombreTransactionsOk()}");
                    ecrivain.WriteLine($"Nombre d'échecs : {Transaction.GetNombreTransactionsKo()}");
                    ecrivain.WriteLine($"Montant total des réussites : {Transaction.GetMontantTransactionsOk()} euros");
                    ecrivain.WriteLine();
                    ecrivain.WriteLine("Frais de gestion :");

                    foreach (Gestionnaire gest in gestionnaires)
                    {
                        ecrivain.WriteLine($"{gest.GetIdentifiant()} : {gest.GetFraisGestion()} euros");
                    }

                }
            }
        }
    }
}

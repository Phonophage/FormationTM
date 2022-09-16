using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Gestionnaire> gestionnaires = LireFichierGest("Gestionnaires.csv");
            List<Operation> operations = LireFichierComptes(gestionnaires, "Comptes.csv");
            List<Transaction> transactions = LireFichierTransactions("Transactions.csv");

            FaireOperations(gestionnaires, operations);
            FaireTransactions(gestionnaires, transactions);

            EcrireSortieOperations(operations, "Statut operations.csv");
            EcrireSortieTransactions(transactions, "Statut transactions.csv");
            EcrireSortieMetrologie(gestionnaires, "Métrologie.txt");
        }

        static bool GestExiste(List<Gestionnaire> gestionnaires, int identifiant)
        {
            foreach (Gestionnaire gest in gestionnaires)
            {
                if (gest.GetIdentifiant() == identifiant)
                {
                    return true;
                }
            }
            return false;
        }

        static int TrouverGest(List<Gestionnaire> gestionnaires, int identifiant)
        {
            for (int i = 0; i < gestionnaires.Count(); i++)
            {
                if (gestionnaires[i].GetIdentifiant() == identifiant)
                {
                    return i;
                }
            }
            return -1;
        }

        static bool CompteExiste(List<Gestionnaire> gestionnaires, int identifiant)
        {
            foreach (Gestionnaire gest in gestionnaires)
            {
                if (gest.CompteExiste(identifiant))
                {
                    return true;
                }
            }
            return false;
        }

        static int[] TrouverCompte(List<Gestionnaire> gestionnaires, int identifiant)
        {
            int[] adresse = new int[] { -1, -1 };

            for (int i = 0; i < gestionnaires.Count(); i++)
            {
                if (gestionnaires[i].CompteExiste(identifiant))
                {
                    adresse[0] = i;
                    adresse[1] = gestionnaires[i].TrouverCompte(identifiant);
                }
            }
            return adresse;
        }

        static bool TransactionExiste(List<Transaction> transactions, int identifiant)
        {
            foreach (Transaction tran in transactions)
            {
                if (tran.GetIdentifiant() == identifiant)
                {
                    return true;
                }
            }
            return false;
        }


        static List<Gestionnaire> LireFichierGest(string path)
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

        static List<Gestionnaire> ParseDataGest(List<string> dataGest)
        {
            List<Gestionnaire> gestionnaires = new List<Gestionnaire>();

            foreach (string ligne in dataGest)
            {
                string[] data = ligne.Split(';');

                if (data.Length == 3) // 3 champs renseignés : identifiant, type, nombre transactions
                {
                    int identifiant = int.Parse(data[0]);

                    if (!GestExiste(gestionnaires, identifiant))
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

        static List<Operation> LireFichierComptes(List<Gestionnaire> gestionnaires,  string path)
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

        static List<Operation> ParseDataComptes(List<Gestionnaire> gestionnaires, List<string> dataComptes)
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

        static void FaireOperations(List<Gestionnaire> gestionnaires, List<Operation> operations)
        {
            foreach (Operation ope in operations)
            {
                ope.SetStatut(OperationCompte(gestionnaires, ope));
            }
        }

        static bool OperationCompte(List<Gestionnaire> gestionnaires, Operation ope)
        {
            int gest_in = TrouverGest(gestionnaires, ope.GetEntree());
            int gest_out = TrouverGest(gestionnaires, ope.GetSortie());

            if (ope.GetEntree() != -1 && ope.GetSortie() == -1) // ouverture de compte
            {
                if (!CompteExiste(gestionnaires, ope.GetIdentifiant())) // on vérifie que le compte n'existe pas
                {
                    gestionnaires[gest_in].AddCompte(new Compte(ope.GetIdentifiant(), ope.GetDate(), ope.GetSolde()));
                    Compte.SetNombreComptes(Compte.GetNombreComptes() + 1);

                    return true;
                }

                return false;
            }
            else if (ope.GetEntree() == -1 && ope.GetSortie() != -1) // fermeture de compte
            {
                if (CompteExiste(gestionnaires, ope.GetIdentifiant())) // on vérifie que le compte existe
                {
                    gestionnaires[gest_out].CloseCompte(ope.GetIdentifiant(), ope.GetDate());
                    Compte.SetNombreComptes(Compte.GetNombreComptes() - 1);

                    return true;
                }

                return false;
            }
            else if (ope.GetEntree() != -1 && ope.GetSortie() != -1) // transfert de compte (a vérifier)
            {
                if (GestExiste(gestionnaires, ope.GetEntree()) && GestExiste(gestionnaires, ope.GetSortie()) && gestionnaires[gest_in].CompteExiste(ope.GetIdentifiant()) &&
                    gestionnaires[gest_in].GetCompte(ope.GetIdentifiant()).IsActif()) // on vérifie l'existance des deux gestionnaires et du compte à transférer, et on vérifie que le compte est actif
                gestionnaires[gest_in].AddCompte(gestionnaires[gest_out].GetCompte(ope.GetIdentifiant()));
                gestionnaires[gest_out].DelCompte(ope.GetIdentifiant());

                return true;
            }

            return false;
        }

        static List<Transaction> LireFichierTransactions(string path)
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

        static List<Transaction> ParseDataTransactions(List<string> dataTransactions)
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
                    bool doublon = TransactionExiste(transactions, identifiant);

                    transactions.Add(new Transaction(identifiant, date, expediteur, destinataire, montant, doublon));
                }
            }

            return transactions;
        }

        static void FaireTransactions(List<Gestionnaire> gestionnaires, List<Transaction> transactions)
        {
            foreach (Transaction tran in transactions)
            {
                tran.SetStatut(Traiter(tran, gestionnaires));
            }
        }

        static bool Traiter(Transaction tran, List<Gestionnaire> gestionnaires)
        {
            if (tran.IsDoublon())
            {
                return false;
            }
            else if (tran.GetDestinataire() != 0 || tran.GetExpediteur() != 0) // on traite la transaction à moins que l'environnement soit à la fois expéditeur et destinataire
            {
                return TraiterTransaction(tran, gestionnaires);
            }
            return false;
        }

        static bool TraiterTransaction(Transaction tran, List<Gestionnaire> gestionnaires)
        {
            if (tran.GetMontant() > 0)
            {
                int[] expediteur = TrouverCompte(gestionnaires, tran.GetExpediteur());
                int[] destinataire = TrouverCompte(gestionnaires, tran.GetDestinataire());
                int gest_exp = expediteur[0];
                int gest_des = destinataire[0];
                int compte_exp = expediteur[1];
                int compte_des = destinataire[1];


                if (gest_exp != -1 && compte_exp != -1 && gest_des != -1 && compte_des != -1 && // l'expéditeur et le destinataire existent
                    tran.GetMontant() <= gestionnaires[gest_exp].GetCompte(tran.GetExpediteur()).GetSolde() && // le montant de la transaction est inférieur ou égal au solde de l'expéditeur
                    gestionnaires[gest_exp].GetCompte(tran.GetExpediteur()).TransactionIsValid(tran)) // la transaction ne dépasse pas le maximum de retrait
                {
                    if (tran.GetExpediteur() != 0) // l'expéditeur n'est pas l'environnement
                    {
                        // maj du solde de l'expéditeur
                        gestionnaires[gest_exp].GetCompte(tran.GetExpediteur()).SetSolde(gestionnaires[gest_exp].GetCompte(tran.GetExpediteur()).GetSolde() - tran.GetMontant());
                        gestionnaires[gest_exp].GetCompte(tran.GetExpediteur()).AddTransaction(tran);
                    }
                    if (tran.GetDestinataire() != 0) // le destinataire n'est pas l'environnement
                    {
                        if (gest_exp == gest_des) // virement entre deux comptes d'un même gestionnaire
                        {
                            gestionnaires[gest_des].GetCompte(tran.GetDestinataire()).SetSolde(gestionnaires[gest_des].GetCompte(tran.GetDestinataire()).GetSolde() + tran.GetMontant());
                        }
                        else
                        {
                            gestionnaires[gest_des].GetCompte(tran.GetDestinataire()).SetSolde((gestionnaires[gest_des].GetCompte(tran.GetDestinataire()).GetSolde() +
                                tran.GetMontant()) - gestionnaires[gest_exp].FraisGestion(tran.GetMontant()));
                            gestionnaires[gest_exp].SetFraisGestion(gestionnaires[gest_exp].GetFraisGestion() + gestionnaires[gest_exp].FraisGestion(tran.GetMontant()));
                        }
                        gestionnaires[gest_des].GetCompte(tran.GetDestinataire()).AddTransaction(tran);
                    }

                    Transaction.SetNombreTransactions(Transaction.GetNombreTransactions() + 1);
                    Transaction.SetNombreTransactionsOk(Transaction.GetNombreTransactionsOk() + 1);
                    Transaction.SetMontantTransactionsOk(Transaction.GetMontantTransactionsOk() + tran.GetMontant());
                    return true;
                }
            }

            Transaction.SetNombreTransactions(Transaction.GetNombreTransactions() + 1);
            Transaction.SetNombreTransactionsKo(Transaction.GetNombreTransactionsKo() + 1);
            return false;
        }

        static void EcrireSortieOperations(List<Operation> operations, string path)
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

        static void EcrireSortieTransactions(List<Transaction> transactions, string path)
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

        static void EcrireSortieMetrologie(List<Gestionnaire> gestionnaires, string path)
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

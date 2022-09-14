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
            // lecture des fichiers d'entrées pour obtenir la liste des comptes et des transactions

            List<Compte> comptes = LectureFichierComptes(args[0]);
            List<Transaction> transactions = LectureFichierTransactions(args[1]);

            // traitement des transactions pour obtenir leur statut

            FaireTransactions(transactions, comptes);

            // ecriture des statuts dans le fichier de sortie

            EcrireFichierSortie(transactions, args[2]);
        }

        // renvoie true si il existe un compte avec cet identifiant dans la liste
        static bool CompteExiste(List<Compte> comptes, int identifiant)
        {
            foreach (Compte cpt in comptes)
            {
                if (cpt.GetIdentifiant() == identifiant)
                {
                    return true;
                }
            }
            return false;
        }

        // renvoie l'indice du compte correspondant à l'identifiant, ou -1 si le compte n'existe pas
        static int TrouverCompte(List<Compte> comptes, int identifiant)
        {
            for (int i = 0; i < comptes.Count(); i++)
            {
                if (comptes[i].GetIdentifiant() == identifiant)
                {
                    return i;
                }
            }
            return -1;
        }

        // renvoie true si il existe une transaction avec cet identifiant dans la liste
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

        // lit le fichier Comptes.txt pour produire une liste de strings qui sera ensuite traduite en liste de comptes par ParseDataComptes
        static List<Compte> LectureFichierComptes(string path)
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

            return ParseDataComptes(dataComptes);
        }

        // traduit une liste de string ou chaque string est au format "identifiant;solde" en liste de comptes
        static List<Compte> ParseDataComptes(List<string> dataComptes)
        {
            List<Compte> comptes = new List<Compte>();

            foreach (string ligne in dataComptes)
            {
                string[] data = ligne.Split(';'); // sépare la string en plusieurs en enlevant les ';'
                int identifiant = int.Parse(data[0]);

                if (!CompteExiste(comptes, identifiant))
                {
                    // on vérifie si le solde du compte est renseigné ou pas. s'il ne l'est pas le solde par défaut est 0
                    if (data[1].Length != 0)
                    {
                        double solde = double.Parse(data[1].Replace('.', ','));
                        if (solde >= 0)
                        {
                            comptes.Add(new Compte(identifiant, solde));
                        }
                    }
                    else
                    {
                        comptes.Add(new Compte(identifiant));
                    }
                }
            }

            return comptes;
        }

        // lit le fichier Transactions.txt pour produire une liste de strings qui sera ensuite traduite en liste de transactions par ParseDataTransactions
        static List<Transaction> LectureFichierTransactions(string path)
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

        // traduit une liste de string ou chaque string est au format "identifiant;montant;expediteur;destinataire" en liste de transactions
        static List<Transaction> ParseDataTransactions(List<string> dataTransactions)
        {
            List<Transaction> transactions = new List<Transaction>();

            foreach (string ligne in dataTransactions)
            {
                string[] data = ligne.Split(';'); // découpe la string en plusieurs strings en enlevant les ';'
                int identifiant = int.Parse(data[0]);
                double montant = double.Parse(data[1]);
                int expediteur = int.Parse(data[2]);
                int destinataire = int.Parse(data[3]);

                if (!TransactionExiste(transactions, identifiant))
                {
                    transactions.Add(new Transaction(identifiant, expediteur, destinataire, montant, false, false));
                }
                else
                {
                    transactions.Add(new Transaction(identifiant, expediteur, destinataire, montant, false, true));
                }
            }

            return transactions;
        }

        // traite chaque transaction pour déterminer son statut
        static void FaireTransactions(List<Transaction> transactions, List<Compte> comptes)
        {
            foreach (Transaction tran in transactions)
            {
                tran.SetStatut(Traiter(tran, comptes));
            }
        }

        // détermine le type de transaction (retrait / depot / virement).
        // renvoie false si la transaction ne correspond à aucun type (virement environnement -> environnement)
        static bool Traiter(Transaction tran, List<Compte> comptes)
        {
            if (tran.IsDoublon())
            {
                return false;
            }
            if (tran.GetDestinataire() == 0 && tran.GetExpediteur() != 0)
            {
                return TraiterRetrait(tran, comptes);
            }
            else if (tran.GetDestinataire() != 0 && tran.GetExpediteur() == 0)
            {
                return TraiterDepot(tran, comptes);
            }
            else if (tran.GetDestinataire() != 0 && tran.GetExpediteur() != 0)
            {
                return TraiterVirement(tran, comptes);
            }
            return false;
        }

        // traite une transaction de type retrait. renvoie false si la transaction est KO.
        static bool TraiterRetrait(Transaction tran, List<Compte> comptes)
        {
            if (tran.GetMontant() > 0) // le montant doit être strictement positif
            {
                int expediteur = TrouverCompte(comptes, tran.GetExpediteur());

                // le compte expéditeur doit exister, le montant doit être inférieur ou égal à son solde,
                // et le montant des 10 dernières transactions ne doit pas dépasser le montant max
                if (expediteur != -1 && tran.GetMontant() <= comptes[expediteur].GetSolde() && comptes[expediteur].TransactionIsValid(tran))
                {
                    comptes[expediteur].SetSolde(comptes[expediteur].GetSolde() - tran.GetMontant());
                    comptes[expediteur].AddTransaction(tran);
                    return true;
                }
            }
            return false;
        }

        // traite une transaction de type depot. renvoie false si la transaction est KO.
        static bool TraiterDepot(Transaction tran, List<Compte> comptes)
        {
            if (tran.GetMontant() > 0) // le montant doit être strictement positif
            {
                int destinataire = TrouverCompte(comptes, tran.GetDestinataire());

                // le compte destinataire doit exister
                if (destinataire != -1)
                {
                    comptes[destinataire].SetSolde(comptes[destinataire].GetSolde() + tran.GetMontant());
                    comptes[destinataire].AddTransaction(tran);
                    return true;
                }
            }
            return false;
        }

        // traite une transaction de type virement. renvoie false si la transaction est KO
        static bool TraiterVirement(Transaction tran, List<Compte> comptes)
        {
            if (tran.GetMontant() > 0) // le montant doit être strictement positif
            {
                int expediteur = TrouverCompte(comptes, tran.GetExpediteur());
                int destinataire = TrouverCompte(comptes, tran.GetDestinataire());

                // les comptes doivent exister, le montant doit être inférieur ou égal au solde de l'expéditeur,
                // et le montant des 10 dernières transactions de l'expéditeur ne doit pas dépasser le montant max
                if (expediteur != -1 && destinataire != -1 && tran.GetMontant() <= comptes[expediteur].GetSolde() && comptes[expediteur].TransactionIsValid(tran))
                {
                    comptes[expediteur].SetSolde(comptes[expediteur].GetSolde() - tran.GetMontant());
                    comptes[destinataire].SetSolde(comptes[destinataire].GetSolde() + tran.GetMontant());
                    comptes[expediteur].AddTransaction(tran);
                    comptes[destinataire].AddTransaction(tran);
                    return true;
                }
            }
            return false;
        }

        // ecrit dans le fichier sortie la liste des statuts des transactions au format "identifiant;statut"
        static void EcrireFichierSortie(List<Transaction> transactions, string path)
        {
            using (FileStream output = File.OpenWrite(path))
            {
                using (StreamWriter ecrivain = new StreamWriter(output))
                {
                    foreach (Transaction tran in transactions)
                    {
                        if (tran.GetStatut())
                        {
                            ecrivain.WriteLine($"{tran.GetIdentifiant()};OK");
                        }
                        else
                        {
                            ecrivain.WriteLine($"{tran.GetIdentifiant()};KO");
                        }
                    }
                }
            }
        }
    }
}

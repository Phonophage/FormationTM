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
            List<Gestionnaire> gestionnaires = Entree.LireFichierGest("Gestionnaires.csv");
            List<Operation> operations = Entree.LireFichierComptes(gestionnaires, "Comptes.csv");
            List<Transaction> transactions = Entree.LireFichierTransactions("Transactions.csv");

            FaireOperations(gestionnaires, operations);
            FaireTransactions(gestionnaires, transactions);

            Sortie.EcrireSortieOperations(operations, "Statut operations.csv");
            Sortie.EcrireSortieTransactions(transactions, "Statut transactions.csv");
            Sortie.EcrireSortieMetrologie(gestionnaires, "Métrologie.txt");
        }

        static void FaireOperations(List<Gestionnaire> gestionnaires, List<Operation> operations)
        {
            foreach (Operation ope in operations)
            {
                ope.SetStatut(OperationCompte(ope, gestionnaires));
            }
        }

        static void FaireTransactions(List<Gestionnaire> gestionnaires, List<Transaction> transactions)
        {
            foreach (Transaction tran in transactions)
            {
                tran.SetStatut(Traiter(tran, gestionnaires));
            }
        }

        static bool OperationCompte(Operation ope, List<Gestionnaire> gestionnaires)
        {
            int gest_in = Outils.TrouverGest(gestionnaires, ope.GetEntree());
            int gest_out = Outils.TrouverGest(gestionnaires, ope.GetSortie());

            if (ope.GetEntree() != -1 && ope.GetSortie() == -1) // ouverture de compte
            {
                if (!Outils.CompteExiste(gestionnaires, ope.GetIdentifiant())) // on vérifie que le compte n'existe pas déjà
                {
                    gestionnaires[gest_in].AddCompte(new Compte(ope.GetIdentifiant(), ope.GetDate(), ope.GetSolde()));
                    Compte.SetNombreComptes(Compte.GetNombreComptes() + 1);

                    return true;
                }

                return false;
            }
            else if (ope.GetEntree() == -1 && ope.GetSortie() != -1) // fermeture de compte
            {
                if (Outils.CompteExiste(gestionnaires, ope.GetIdentifiant())) // on vérifie que le compte existe bien
                {
                    gestionnaires[gest_out].CloseCompte(ope.GetIdentifiant(), ope.GetDate());
                    Compte.SetNombreComptes(Compte.GetNombreComptes() - 1);

                    return true;
                }

                return false;
            }
            else if (ope.GetEntree() != -1 && ope.GetSortie() != -1) // transfert de compte (a vérifier)
            {
                if (Outils.GestExiste(gestionnaires, ope.GetEntree()) && Outils.GestExiste(gestionnaires, ope.GetSortie()) && gestionnaires[gest_in].CompteExiste(ope.GetIdentifiant()) &&
                    gestionnaires[gest_in].GetCompte(ope.GetIdentifiant()).IsActif()) // on vérifie l'existance des deux gestionnaires et du compte à transférer, et on vérifie que le compte est actif
                {
                    gestionnaires[gest_in].AddCompte(gestionnaires[gest_out].GetCompte(ope.GetIdentifiant()));
                    gestionnaires[gest_out].DelCompte(ope.GetIdentifiant());

                    return true;
                }
            }

            return false;
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
                int[] expediteur = Outils.TrouverCompte(gestionnaires, tran.GetExpediteur());
                int[] destinataire = Outils.TrouverCompte(gestionnaires, tran.GetDestinataire());
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
    }
}

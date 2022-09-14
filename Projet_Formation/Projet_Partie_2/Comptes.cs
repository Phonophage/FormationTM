using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    class Compte
    {
        private readonly int identifiant;
        private double solde;
        private readonly double maxRetrait;
        private List<Transaction> transactions;

        public Compte(int id, double sld = 0, double maxR = 1000)
        {
            identifiant = id;
            solde = sld;
            maxRetrait = maxR;
            transactions = new List<Transaction>();
        }

        public int GetIdentifiant()
        {
            return identifiant;
        }

        public double GetSolde()
        {
            return solde;
        }

        public void SetSolde(double montant)
        {
            solde = montant;
        }

        public double GetMaxRetrait()
        {
            return maxRetrait;
        }

        public List<Transaction> GetTransactions()
        {
            return transactions;
        }

        public void AddTransaction(Transaction tran)
        {
            transactions.Add(tran);
        }

        // renvoie true si la somme des 9 dernières transactions + la transaction en paramètre est inférieure au montant de retrait maximum
        public bool TransactionIsValid(Transaction tran)
        {
            if (tran.GetMontant() + TenLastTransactions() > maxRetrait)
            {
                return false;
            }
            return true;
        }

        // renvoie la somme des 9 dernières transactions
        private double TenLastTransactions()
        {
            double total = 0;

            for (int i = transactions.Count() - 1; i > 0 && i > transactions.Count() - 10; i--)
            {
                if (transactions[i].GetExpediteur() == identifiant)
                {
                    total += transactions[i].GetMontant();
                }
            }

            return total;
        }
    }
}

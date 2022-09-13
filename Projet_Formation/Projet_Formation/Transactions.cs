using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    class Transaction
    {
        private readonly int identifiant;
        private double montant;
        private int expediteur;
        private int destinataire;

        public Transaction(int id, int ex, int de, double mo = 0)
        {
            identifiant = id;
            montant = mo;
            expediteur = ex;
            destinataire = de;
        }

        public int GetIdentifiant()
        {
            return identifiant;
        }

        public double GetMontant()
        {
            return montant;
        }

        public int GetExpediteur()
        {
            return expediteur;
        }

        public int GetDestinataire()
        {
            return destinataire;
        }
    }
}

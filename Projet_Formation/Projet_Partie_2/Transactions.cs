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
        private bool statut;
        private bool is_doublon;

        public Transaction(int id, int ex, int de, double mo, bool st, bool db)
        {
            identifiant = id;
            montant = mo;
            expediteur = ex;
            destinataire = de;
            statut = st;
            is_doublon = db;
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

        public bool GetStatut()
        {
            return statut;
        }

        public void SetStatut(bool s)
        {
            statut = s;
        }

        public bool IsDoublon()
        {
            return is_doublon;
        }
    }
}

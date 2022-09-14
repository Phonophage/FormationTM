using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    abstract class Gestionnaire
    {
        private int _identifiant;
        
        private int _dernieres_transactions;
        private double _frais_gestion;
        
        private List<Compte> _comptes;

        public Gestionnaire(int id, int dt = 10, double fg = 0)
        {
            _identifiant = id;
            _dernieres_transactions = dt;
            _frais_gestion = fg;
            _comptes = new List<Compte>();
        }

        public int GetIdentifiant()
        {
            return _identifiant;
        }

        public int GetDernieresTransactions()
        {
            return _dernieres_transactions;
        }

        public double GetFraisGestion()
        {
            return _frais_gestion;
        }

        public void SetFraisGestion(double montant)
        {
            _frais_gestion = montant;
        }

        public List<Compte> GetComptes()
        {
            return _comptes;
        }

        public void AddCompte(Compte cpt)
        {
            _comptes.Add(cpt);
        }

        public bool DelCompte(int identifiant)
        {
            foreach(Compte cpt in _comptes)
            {
                if (cpt.GetIdentifiant() == identifiant)
                {
                    _comptes.Remove(cpt);
                    return true;
                }
            }
            return false;
        }
    }
}

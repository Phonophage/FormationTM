using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    abstract class Gestionnaire
    {
        protected int _identifiant;
        
        protected int _nombre_transactions;
        protected double _frais_gestion;
        
        protected List<Compte> _comptes;

        public Gestionnaire(int identifiant, int nombre_transactions, double frais_gestion)
        {
            _identifiant = identifiant;
            _nombre_transactions = nombre_transactions;
            _frais_gestion = frais_gestion;
            _comptes = new List<Compte>();
        }

        public int GetIdentifiant()
        {
            return _identifiant;
        }

        public int GetDernieresTransactions()
        {
            return _nombre_transactions;
        }

        public double GetFraisGestion()
        {
            return _frais_gestion;
        }

        public void SetFraisGestion(double montant)
        {
            _frais_gestion = montant;
        }

        public void AddFraisGestion(double montant)
        {
            _frais_gestion += montant;
        }

        public abstract double FraisGestion(double montant);

        public List<Compte> GetComptes()
        {
            return _comptes;
        }

        public Compte GetCompte(int identifiant)
        {
            foreach(Compte cpt in _comptes)
            {
                if (cpt != null && cpt.GetIdentifiant() == identifiant)
                {
                    return cpt;
                }
            }
            return null;
        }

        public bool CompteExiste(int identifiant)
        {
            foreach (Compte cpt in _comptes)
            {
                if (cpt != null && cpt.GetIdentifiant() == identifiant)
                {
                    return true;
                }
            }
            return false;
        }

        public int TrouverCompte(int identifiant)
        {
            for (int i = 0; i < _comptes.Count(); i++)
            {
                if (_comptes[i].GetIdentifiant() == identifiant)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddCompte(Compte cpt)
        {
            if (cpt != null)
            {
                _comptes.Add(cpt);
            }
        }

        public void CloseCompte(int identifiant, DateTime date)
        {
            foreach (Compte cpt in _comptes)
            {
                if (cpt != null && cpt.GetIdentifiant() == identifiant && cpt.GetDateResiliation() == DateTime.MaxValue)
                {
                    cpt.FermerCompte(date);
                }
            }
        }

        public bool DelCompte(int identifiant)
        {
            foreach (Compte cpt in _comptes)
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

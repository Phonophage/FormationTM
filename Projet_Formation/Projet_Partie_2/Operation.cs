using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Operation
    {
        private int _identifiant;
        private DateTime _date;
        private double _solde;
        private int _entree;
        private int _sortie;

        private bool _statut;

        public Operation(int id, DateTime date, double solde, int entree, int sortie, bool statut = false)
        {
            _identifiant = id;
            _date = date;
            _solde = solde;
            _entree = entree;
            _sortie = sortie;
            _statut = statut;

        }

        public int GetIdentifiant()
        {
            return _identifiant;
        }

        public DateTime GetDate()
        {
            return _date;
        }

        public double GetSolde()
        {
            return _solde;
        }

        public int GetEntree()
        {
            return _entree;
        }

        public int GetSortie()
        {
            return _sortie;
        }

        public bool GetStatut()
        {
            return _statut;
        }

        public void SetStatut(bool statut)
        {
            _statut = statut;
        }
    }
}

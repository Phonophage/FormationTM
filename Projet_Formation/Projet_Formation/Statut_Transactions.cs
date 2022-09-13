using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    class Statut
    {
        private readonly int identifiant;
        private bool is_ok;

        public Statut(int id, bool statut)
        {
            identifiant = id;
            is_ok = statut;
        }

        public int GetIdentifiant()
        {
            return identifiant;
        }

        public bool GetStatut()
        {
            return is_ok;
        }
    }
}

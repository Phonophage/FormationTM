using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Particulier : Gestionnaire
    {
        private const double _frais = 1;

        public Particulier(int identifiant, int nombre_transactions, double frais_gestion = 0) : base(identifiant, nombre_transactions, frais_gestion) { }

        public override double FraisGestion(double montant)
        {
            return (montant * _frais ) / 100;
        }
    }
}

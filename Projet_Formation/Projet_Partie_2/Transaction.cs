using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_2
{
    class Transaction
    {
        static int _nombre_transactions;
        static int _nombre_transactions_ok;
        static int _nombre_transactions_ko;
        static double _montant_transactions_ok;

        private readonly int _identifiant;

        private double _montant;
        private int _expediteur;
        private int _destinataire;

        private bool _statut;
        private bool _is_doublon;

        private DateTime _date_effet;

        static Transaction()
        {
            _nombre_transactions_ok = 0;
            _montant_transactions_ok = 0;
        }

        static int GetNombreTransactions()
        {
            return _nombre_transactions;
        }

        static void SetNombreTransactions(int n)
        {
            _nombre_transactions = n;
        }

        static int GetNombreTransactionsOk()
        {
            return _nombre_transactions_ok;
        }

        static void SetNombreTransactionsOk(int n)
        {
            _nombre_transactions_ok = n;
        }

        static int GetNombreTransactionsKo()
        {
            return _nombre_transactions_ko;
        }

        static void SetNombreTransactionsKo(int n)
        {
            _nombre_transactions_ko = n;
        }

        static double GetMontantTransactionsOk()
        {
            return _montant_transactions_ok;
        }

        static void SetMontantTransactionOk(double m)
        {
            _montant_transactions_ok = m;
        }

        public Transaction(int id, DateTime ef, int ex, int de, double mo, bool st, bool db)
        {
            _identifiant = id;
            _date_effet = ef;
            _montant = mo;
            _expediteur = ex;
            _destinataire = de;
            _statut = st;
            _is_doublon = db;
        }

        public int GetIdentifiant()
        {
            return _identifiant;
        }

        public DateTime GetDateEffet()
        {
            return _date_effet;
        }

        public double GetMontant()
        {
            return _montant;
        }

        public int GetExpediteur()
        {
            return _expediteur;
        }

        public int GetDestinataire()
        {
            return _destinataire;
        }

        public bool GetStatut()
        {
            return _statut;
        }

        public void SetStatut(bool s)
        {
            _statut = s;
        }

        public bool IsDoublon()
        {
            return _is_doublon;
        }
    }
}

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

        private DateTime _date;

        static Transaction()
        {
            _nombre_transactions = 0;
            _nombre_transactions_ok = 0;
            _nombre_transactions_ko = 0;
            _montant_transactions_ok = 0;
        }

        public static int GetNombreTransactions()
        {
            return _nombre_transactions;
        }

        public static void SetNombreTransactions(int n)
        {
            _nombre_transactions = n;
        }

        public static int GetNombreTransactionsOk()
        {
            return _nombre_transactions_ok;
        }

        public static void SetNombreTransactionsOk(int n)
        {
            _nombre_transactions_ok = n;
        }

        public static int GetNombreTransactionsKo()
        {
            return _nombre_transactions_ko;
        }

        public static void SetNombreTransactionsKo(int n)
        {
            _nombre_transactions_ko = n;
        }

        public static double GetMontantTransactionsOk()
        {
            return _montant_transactions_ok;
        }

        public static void SetMontantTransactionsOk(double m)
        {
            _montant_transactions_ok = m;
        }

        public Transaction(int identifiant, DateTime date, int expediteur, int destinataire, double montant, bool doublon, bool statut = false)
        {
            _identifiant = identifiant;
            _date = date;
            _montant = montant;
            _expediteur = expediteur;
            _destinataire = destinataire;
            _statut = statut;
            _is_doublon = doublon;
        }

        public int GetIdentifiant()
        {
            return _identifiant;
        }

        public DateTime GetDate()
        {
            return _date;
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

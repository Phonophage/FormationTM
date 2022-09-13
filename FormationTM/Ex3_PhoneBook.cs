using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serie_IV
{
    public class PhoneBook
    {
        //1. La structure de donnée adaptée est le dictionnaire, car on aura des clés uniques (numéros de téléphone) et des valeurs (noms de contact)
        
        private Dictionary<string, string> annuaire;

        public PhoneBook()
        {
            annuaire = new Dictionary<string, string>();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return (phoneNumber.Length == 10 && phoneNumber[0] == '0' && phoneNumber[1] != '0');
        }

        public bool ContainsPhoneContact(string phoneNumber)
        {
            return (annuaire.ContainsKey(phoneNumber));
        }

        public void PhoneContact(string phoneNumber)
        {
            if (ContainsPhoneContact(phoneNumber))
            {
                Console.WriteLine($"{phoneNumber} : {annuaire[phoneNumber]}");
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public bool AddPhoneNumber(string phoneNumber, string name)
        {
            if (!ContainsPhoneContact(phoneNumber) && IsValidPhoneNumber(phoneNumber))
            {
                annuaire.Add(phoneNumber, name);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeletePhoneNumber(string phoneNumber)
        {
            return (annuaire.Remove(phoneNumber));
        }

        public void DisplayPhoneBook()
        {
            Console.WriteLine("Annuaire téléphonique :");
            if (annuaire.Count() == 0)
            {
                Console.WriteLine("Pas de numéros téléphoniques");
            }
            foreach(KeyValuePair<string, string> personne in annuaire)
            {
                PhoneContact(personne.Key);
            }
        }
    }
}

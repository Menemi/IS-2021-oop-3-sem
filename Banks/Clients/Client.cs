using System.Collections.Generic;
using Banks.AccountTypes;

namespace Banks
{
    public class Client : ClientBuilder
    {
        private List<Account> _accountsList;

        public Client(string name, string surname)
        {
            _accountsList = new List<Account>();
            Name = name;
            Surname = surname;
        }

        public string Name { get; private set; }

        public string Surname { get; private set; }

        public string Address { get; private set; } = null;

        public Passport Passport { get; private set; } = null;

        public bool Doubtful { get; private set; } = true;

        public override void SetName(string name)
        {
            Name = name;
        }

        public override void SetSurname(string surname)
        {
            Surname = surname;
        }

        public override void SetAddress(string address)
        {
            Address = address;
            if (Passport != null)
            {
                Doubtful = false;
            }
        }

        public override void SetPassport(Passport passport)
        {
            Passport = passport;
            if (Address != null)
            {
                Doubtful = false;
            }
        }

        public void AddNewAccount(Account account)
        {
            _accountsList.Add(account);
        }
    }
}
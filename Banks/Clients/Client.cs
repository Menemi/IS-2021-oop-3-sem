using System.Collections.Generic;
using Banks.AccountTypes;

namespace Banks
{
    public class Client : ClientBuilder
    {
        public override void SetName(string name)
        {
            Person.Name = name;
        }

        public override void SetSurname(string surname)
        {
            Person.Surname = surname;
        }

        public override void SetAddress(string address)
        {
            Person.Address = address;
            if (Person.Passport != null)
            {
                Person.Doubtful = false;
            }
        }

        public override void SetPassport(Passport passport)
        {
            Person.Passport = passport;
            if (Person.Address != null)
            {
                Person.Doubtful = false;
            }
        }

        public void AddNewAccount(Account account)
        {
            Person.AddNewAccount(account);
        }
    }
}
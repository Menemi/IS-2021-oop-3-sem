using System.Collections.Generic;
using System.Collections.ObjectModel;
using Banks.AccountTypes;
using Banks.Observers;

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
    }
}
using System.Collections.Generic;
using Banks.AccountTypes;

namespace Banks
{
    public class Person
    {
        private List<Account> _accountsList;

        public Person(string name, string surname)
        {
            _accountsList = new List<Account>();
            Name = name;
            Surname = surname;
        }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; } = null;

        public Passport Passport { get; set; } = null;

        public bool Doubtful { get; set; } = true;

        public void AddNewAccount(Account account)
        {
            _accountsList.Add(account);
        }
    }
}
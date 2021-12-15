using System;
using System.Collections.Generic;
using Banks.AccountTypes;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var list = new List<PercentOfTheAmount>()
            {
                new PercentOfTheAmount(0, float.MaxValue, 3),
            };

            var bank = new Bank("Tinkoff", list, 3, 10000, 10000, 10000, 100, new DateTime(2022, 1, 1));
            AccountBuilder builder = new DebitAccount();
            var debit = bank.CreateAccount(builder, 0);
            builder = new CreditAccount();
            var credit = bank.CreateAccount(builder, 0);
            builder = new DepositAccount();
            var deposit = bank.CreateAccount(builder, 100000);

            Console.Read();
        }
    }
}
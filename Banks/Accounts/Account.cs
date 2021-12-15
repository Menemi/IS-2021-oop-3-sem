using System;
using System.Collections.Generic;

namespace Banks.AccountTypes
{
    public class Account
    {
        private List<Transaction> _transactionHistory;

        public Account(long id)
        {
            Id = id;
            _transactionHistory = new List<Transaction>();
        }

        public long Id { get; }

        public float Percent { get; set; }

        public float Balance { get; private set; }

        public float MaxWithdraw { get; set; } = -1;

        public float MaxRemittance { get; set; } = -1;

        public float CreditLimit { get; set; }

        public float Commission { get; set; }

        public DateTime AccountUnblockingPeriod { get; set; }

        public void IncreaseMoney(float amount)
        {
            Balance += amount;
        }

        public void ReduceMoney(float amount)
        {
            Balance -= amount;
        }
    }
}
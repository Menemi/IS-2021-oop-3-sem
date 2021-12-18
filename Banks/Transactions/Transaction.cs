using System;
using Banks.AccountTypes;

namespace Banks
{
    public abstract class Transaction
    {
        public Transaction(int id, Account sender, Account recipient, float amount)
        {
            Id = id;
            TransactionAmount = amount;
            Sender = sender;
            Recipient = recipient;
            TransactionTime = DateTime.Now;
        }

        public int Id { get; }

        public float TransactionAmount { get; }

        public Account Sender { get; }

        public Account Recipient { get; }

        public DateTime TransactionTime { get; }
    }
}
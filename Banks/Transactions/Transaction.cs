using System;
using Banks.AccountTypes;

namespace Banks
{
    public abstract class Transaction
    {
        private bool _statusCanceled;

        public Transaction(int id, Account sender, Account recipient, double amount)
        {
            Id = id;
            TransactionAmount = amount;
            Sender = sender;
            Recipient = recipient;
            TransactionTime = DateTime.Now;
            _statusCanceled = false;
        }

        public int Id { get; }

        public double TransactionAmount { get; }

        public Account Sender { get; }

        public Account Recipient { get; }

        public DateTime TransactionTime { get; }

        public bool IsCanceled()
        {
            return _statusCanceled;
        }

        public void Cancle()
        {
            _statusCanceled = true;
        }
    }
}
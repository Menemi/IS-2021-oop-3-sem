using System;

namespace Banks
{
    public abstract class Transaction
    {
        public Transaction(float transactionAmount)
        {
            TransactionTime = DateTime.Now;
            TransactionAmount = transactionAmount;
        }

        public DateTime TransactionTime { get; set; }

        public float TransactionAmount { get; set; }
    }
}
using System;
using Banks.AccountTypes;

namespace Banks
{
    public class TransactionReplenishment : Transaction
    {
        public TransactionReplenishment(Account sender, Account recipient, double transactionAmount, int id)
            : base(id, sender, recipient, transactionAmount)
        {
            recipient.IncreaseMoney(transactionAmount);
        }
    }
}
using System;
using Banks.AccountTypes;
using Banks.Exceptions;

namespace Banks
{
    public class TransactionWithdraw : Transaction
    {
        public TransactionWithdraw(Account sender, Account recipient, double transactionAmount, int id)
            : base(id, sender, recipient, -transactionAmount)
        {
            if (recipient.MaxWithdraw != -1 && transactionAmount > recipient.MaxWithdraw)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't withdraw more than {recipient.MaxWithdraw}");
            }

            if (recipient.Balance + recipient.CreditLimit < transactionAmount)
            {
                throw new BanksException(
                    $"You have not enough money (balance: {recipient.Balance}; credit limit:{recipient.CreditLimit})");
            }

            recipient.ReduceMoney(transactionAmount);
        }
    }
}
using Banks.AccountTypes;
using Banks.Exceptions;

namespace Banks
{
    public class TransactionRemittance : Transaction
    {
        public TransactionRemittance(Account sender, Account recipient, double transactionAmount, int id)
            : base(id, sender, recipient, transactionAmount)
        {
            if (sender.MaxRemittance != -1 && transactionAmount > sender.MaxRemittance)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't transfer more than {sender.MaxRemittance}");
            }

            if (sender.Balance < transactionAmount)
            {
                throw new BanksException($"You have not enough money ({sender.Balance})");
            }

            sender.ReduceMoney(transactionAmount);
            recipient.IncreaseMoney(transactionAmount);
        }
    }
}
using Banks.AccountTypes;

namespace Banks
{
    public class TransactionCancellation : Transaction
    {
        public TransactionCancellation(Transaction transaction)
            : base(transaction.Id, transaction.Sender, transaction.Recipient, transaction.TransactionAmount)
        {
            if (transaction.Sender != null)
            {
                transaction.Recipient.ReduceMoney(transaction.TransactionAmount);
                transaction.Sender.IncreaseMoney(transaction.TransactionAmount);
                return;
            }

            if (transaction.TransactionAmount < 0)
            {
                transaction.Recipient.IncreaseMoney(transaction.TransactionAmount);
            }
            else
            {
                transaction.Recipient.ReduceMoney(transaction.TransactionAmount);
            }
        }
    }
}
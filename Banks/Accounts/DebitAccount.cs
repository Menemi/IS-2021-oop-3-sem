using System;
using Banks.Exceptions;

namespace Banks.AccountTypes
{
    public class DebitAccount : AccountBuilder
    {
        private int _transactionIdCounter = 1;

        public override void SetPercent(float percent)
        {
            Account.Percent = percent;
        }

        public override void SetDepositPercent(float percent)
        {
        }

        public override void SetStartBalance(float startBalance)
        {
            Account.IncreaseMoney(startBalance);
        }

        public override void SetMaxWithdraw(float amount)
        {
            Account.MaxWithdraw = amount;
        }

        public override void SetMaxRemittance(float amount)
        {
            Account.MaxRemittance = amount;
        }

        public override void SetCreditLimit(float creditLimit)
        {
        }

        public override void SetCommission(float commission)
        {
        }

        public override void SetAccountUnblockingPeriod(DateTime date)
        {
        }

        public void Replenishment(float amount)
        {
            var transaction = new TransactionReplenishment(null, Account, amount, _transactionIdCounter++);
            Account.NewTransaction(transaction);
        }

        public void Withdraw(float amount)
        {
            var transaction = new TransactionWithdraw(null, Account, amount, _transactionIdCounter++);
            Account.NewTransaction(transaction);
        }

        public void Remittance(Account recipient, float amount)
        {
            var transaction = new TransactionRemittance(Account, recipient, amount, _transactionIdCounter++);
            Account.NewTransaction(transaction);
        }

        public void Cancellation(Transaction oldTransaction)
        {
            var transaction = new TransactionCancellation(oldTransaction);
            Account.NewTransaction(transaction);
        }
    }
}
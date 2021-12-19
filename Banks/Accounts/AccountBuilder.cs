using System;

namespace Banks.AccountTypes
{
    public abstract class AccountBuilder
    {
        public Account Account { get; private set; }

        public void CreateNewAccount(long id)
        {
            Account = new Account(id);
        }

        public abstract void SetPercent(double percent);

        public abstract void SetDepositPercent(double percent);

        public abstract void SetStartBalance(double startBalance);

        public abstract void SetMaxWithdraw(double amount);

        public abstract void SetMaxRemittance(double amount);

        public abstract void SetCreditLimit(double creditLimit);

        public abstract void SetCommission(double commission);

        public abstract void SetAccountUnblockingPeriod(DateTime date);
    }
}
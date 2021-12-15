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

        public abstract void SetPercent(float percent);

        public abstract void SetDepositPercent(float percent);

        public abstract void SetStartBalance(float startBalance);

        public abstract void SetMaxWithdraw(float amount);

        public abstract void SetMaxRemittance(float amount);

        public abstract void SetCreditLimit(float creditLimit);

        public abstract void SetCommission(float commission);

        public abstract void SetAccountUnblockingPeriod(DateTime date);
    }
}
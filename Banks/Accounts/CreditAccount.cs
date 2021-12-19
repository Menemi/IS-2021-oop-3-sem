using System;
using Banks.Exceptions;

namespace Banks.AccountTypes
{
    public class CreditAccount : AccountBuilder
    {
        public override void SetPercent(float percent)
        {
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
            Account.CreditLimit = creditLimit;
            Account.IncreaseMoney(creditLimit);
        }

        public override void SetCommission(float commission)
        {
            Account.Commission = commission;
        }

        public override void SetAccountUnblockingPeriod(DateTime date)
        {
        }
    }
}
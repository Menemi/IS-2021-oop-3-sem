using System;
using Banks.Exceptions;

namespace Banks.AccountTypes
{
    public class CreditAccount : AccountBuilder
    {
        public override void SetPercent(double percent)
        {
        }

        public override void SetDepositPercent(double percent)
        {
        }

        public override void SetStartBalance(double startBalance)
        {
            Account.IncreaseMoney(startBalance);
        }

        public override void SetMaxWithdraw(double amount)
        {
            Account.MaxWithdraw = amount;
        }

        public override void SetMaxRemittance(double amount)
        {
            Account.MaxRemittance = amount;
        }

        public override void SetCreditLimit(double creditLimit)
        {
            Account.CreditLimit = creditLimit;
            Account.IncreaseMoney(creditLimit);
        }

        public override void SetCommission(double commission)
        {
            Account.Commission = commission;
        }

        public override void SetAccountUnblockingPeriod(DateTime date)
        {
        }
    }
}
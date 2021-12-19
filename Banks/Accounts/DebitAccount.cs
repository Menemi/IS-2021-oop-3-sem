using System;
using Banks.Exceptions;

namespace Banks.AccountTypes
{
    public class DebitAccount : AccountBuilder
    {
        public override void SetPercent(double percent)
        {
            Account.Percent = percent;
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
        }

        public override void SetCommission(double commission)
        {
        }

        public override void SetAccountUnblockingPeriod(DateTime date)
        {
        }
    }
}
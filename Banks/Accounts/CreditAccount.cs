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
        }

        public override void SetCommission(float commission)
        {
            Account.Commission = commission;
        }

        public override void SetAccountUnblockingPeriod(DateTime date)
        {
        }

        public void Replenishment(float amount)
        {
            Account.IncreaseMoney(amount);
        }

        public void Withdraw(float amount)
        {
            if (Account.MaxWithdraw != -1 && amount > Account.MaxWithdraw)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't withdraw more than {Account.MaxWithdraw}");
            }

            if (Account.Balance + Account.CreditLimit < amount)
            {
                throw new BanksException(
                    $"You have not enough money (balance: {Account.Balance}; credit limit:{Account.CreditLimit})");
            }

            Account.ReduceMoney(amount);
        }

        public void Remittance(Account account, float amount)
        {
            if (Account.MaxRemittance != -1 && amount > Account.MaxRemittance)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't transfer more than {Account.MaxRemittance}");
            }

            if (Account.Balance + Account.CreditLimit < amount)
            {
                throw new BanksException(
                    $"You have not enough money (balance: {Account.Balance}; credit limit:{Account.CreditLimit})");
            }

            Account.ReduceMoney(amount);
            account.IncreaseMoney(amount);
        }
    }
}
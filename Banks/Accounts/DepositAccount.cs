﻿using System;
using Banks.Exceptions;

namespace Banks.AccountTypes
{
    public class DepositAccount : AccountBuilder
    {
        public override void SetPercent(float percent)
        {
        }

        public override void SetDepositPercent(float percent)
        {
            Account.Percent = percent;
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
            Account.AccountUnblockingPeriod = date;
        }

        public void Replenishment(float amount)
        {
            Account.IncreaseMoney(amount);
        }

        public void Withdraw(float amount)
        {
            if (Account.AccountUnblockingPeriod < DateTime.Now)
            {
                throw new BanksException(
                    $"Unblocking period ({Account.AccountUnblockingPeriod}) did bot come, you can't withdraw any money");
            }

            if (Account.MaxWithdraw != -1 && amount > Account.MaxWithdraw)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't withdraw more than {Account.MaxWithdraw}");
            }

            if (Account.Balance < amount)
            {
                throw new BanksException($"You have not enough money ({Account.Balance})");
            }

            Account.ReduceMoney(amount);
        }

        public void Remittance(Account account, float amount)
        {
            if (Account.AccountUnblockingPeriod < DateTime.Now)
            {
                throw new BanksException(
                    $"Unblocking period ({Account.AccountUnblockingPeriod}) did bot come, you can't transfer any money");
            }

            if (Account.MaxRemittance != -1 && amount > Account.MaxRemittance)
            {
                throw new BanksException(
                    $"Your profile is doubtful you can't transfer more than {Account.MaxRemittance}");
            }

            if (Account.Balance < amount)
            {
                throw new BanksException($"You have not enough money ({Account.Balance})");
            }

            Account.ReduceMoney(amount);
            account.IncreaseMoney(amount);
        }
    }
}
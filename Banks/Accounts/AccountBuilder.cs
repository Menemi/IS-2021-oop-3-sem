using System;

namespace Banks.AccountTypes
{
    public class AccountBuilder
    {
        private Account _account;

        public AccountBuilder(long id)
        {
            _account = new Account(id);
        }

        public static implicit operator Account(AccountBuilder builder)
        {
            return builder._account;
        }

        public AccountBuilder SetPercent(double percent)
        {
            _account.Percent = percent;
            return this;
        }

        public AccountBuilder SetDepositPercent(double percent)
        {
            _account.Percent = percent;
            return this;
        }

        public AccountBuilder SetStartBalance(double startBalance)
        {
            _account.IncreaseMoney(startBalance);
            return this;
        }

        public AccountBuilder SetMaxWithdraw(double amount)
        {
            _account.MaxWithdraw = amount;
            return this;
        }

        public AccountBuilder SetMaxRemittance(double amount)
        {
            _account.MaxRemittance = amount;
            return this;
        }

        public AccountBuilder SetCreditLimit(double creditLimit)
        {
            _account.CreditLimit = creditLimit;
            _account.IncreaseMoney(creditLimit);
            return this;
        }

        public AccountBuilder SetCommission(double commission)
        {
            _account.Commission = commission;
            return this;
        }

        public AccountBuilder SetAccountUnblockingPeriod(DateTime date)
        {
            _account.AccountUnblockingPeriod = date;
            return this;
        }

        public Account Build()
        {
            return _account;
        }
    }
}
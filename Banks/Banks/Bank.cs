using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountTypes;
using Banks.Exceptions;

namespace Banks
{
    public class Bank
    {
        private static int _bankIdCounter = 1;

        private static long _accountIdCounter = 1234567800000000;

        private List<Account> _clientsAccounts;

        private List<PercentOfTheAmount> _percentsOfTheAmount;

        private float _fixedPercent;

        private float _maxWithdrawAmount;

        private float _maxRemittanceAmount;

        private float _creditLimit;

        private float _comission;

        private DateTime _accountUnblockingPeriod;

        public Bank(
            string bankName,
            List<PercentOfTheAmount> percentsOfTheAmount,
            float fixedPercent,
            float maxWithdrawAmount,
            float maxRemittanceAmount,
            float creditLimit,
            float comission,
            DateTime accountUnblockingPeriod)
        {
            Id = _bankIdCounter++;
            Name = bankName;
            _percentsOfTheAmount = percentsOfTheAmount;
            _clientsAccounts = new List<Account>();
            _fixedPercent = fixedPercent;
            _maxWithdrawAmount = maxWithdrawAmount;
            _maxRemittanceAmount = maxRemittanceAmount;
            _creditLimit = creditLimit;
            _comission = comission;
            _accountUnblockingPeriod = accountUnblockingPeriod;
        }

        public int Id { get; }

        public string Name { get; }

        public Account CreateAccount(AccountBuilder accountBuilder, Client client, float startBalance)
        {
            accountBuilder.CreateNewAccount(_accountIdCounter++);
            accountBuilder.SetPercent(_fixedPercent);
            accountBuilder.SetDepositPercent(UpdateDepositPercent(startBalance));
            accountBuilder.SetStartBalance(startBalance);
            accountBuilder.SetMaxWithdraw(_maxWithdrawAmount);
            accountBuilder.SetMaxRemittance(_maxRemittanceAmount);
            accountBuilder.SetCreditLimit(_creditLimit);
            accountBuilder.SetCommission(_comission);
            accountBuilder.SetAccountUnblockingPeriod(_accountUnblockingPeriod);

            _clientsAccounts.Add(accountBuilder.Account);
            client.AddNewAccount(accountBuilder.Account);

            return accountBuilder.Account;
        }

        private float UpdateDepositPercent(float balance)
        {
            foreach (var percent in _percentsOfTheAmount
                .Where(percent => balance >= percent.LowerBound && balance <= percent.UpperBound))
            {
                return percent.Percent;
            }

            throw new BanksException("Bank error, try again later");
        }

        // public void AddNewDepositPercents(float lowerBound, float upperBound, float percent)
        // {
        //     var newPercent = new PercentOfTheAmount(lowerBound, upperBound, percent);
        //     _percentsOfTheAmount.Add(newPercent);
        // }

        // public IAccount CreateCreditAccount(Client client)
        // {
        //     IAccount creditAccount = new CreditAccount(_accountIdCounter++);
        //     if (client.Doubtful)
        //     {
        //         creditAccount.SetMaxRemittance(_maxRemittanceAmount);
        //         creditAccount.SetMaxWithdraw(_maxWithdrawAmount);
        //     }
        //
        //     _clientsAccounts.Add(creditAccount);
        //     client.AddNewAccount(creditAccount);
        //     return creditAccount;
        // }
        //
        // public IAccount CreateDebitAccount(Client client, float startBalance)
        // {
        //     IAccount debitAccount = new DebitAccount(_accountIdCounter++);
        //     if (client.Doubtful)
        //     {
        //         debitAccount.SetMaxRemittance(_maxRemittanceAmount);
        //         debitAccount.SetMaxWithdraw(_maxWithdrawAmount);
        //     }
        //
        //     _clientsAccounts.Add(debitAccount);
        //     client.AddNewAccount(debitAccount);
        //     return debitAccount;
        // }
        //
        // public IAccount CreateDepositAccount(Client client, float startBalance)
        // {
        //     IAccount depositAccount = new DepositAccount(_accountIdCounter++, startBalance);
        //     if (client.Doubtful)
        //     {
        //         depositAccount.SetMaxRemittance(_maxRemittanceAmount);
        //         depositAccount.SetMaxWithdraw(_maxWithdrawAmount);
        //     }
        //
        //     _clientsAccounts.Add(depositAccount);
        //     client.AddNewAccount(depositAccount);
        //     return depositAccount;
        // }
    }
}
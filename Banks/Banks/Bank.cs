using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Banks.AccountTypes;
using Banks.Exceptions;
using Banks.Observers;

namespace Banks
{
    public class Bank : IPercentAccrualObservable, IPercentAccrualObserver
    {
        private static int _bankIdCounter = 1;

        private static long _accountIdCounter = 1234567800000000;

        private List<IPercentAccrualObserver> _clientsAccounts;

        private List<IPercentAccrualObserver> _clientsAccountObservers;

        private List<PercentOfTheAmount> _percentsOfTheAmount;

        private double _fixedPercent;

        private double _maxWithdrawAmount;

        private double _maxRemittanceAmount;

        private double _creditLimit;

        private double _comission;

        private DateTime _accountUnblockingPeriod;

        public Bank(
            string bankName,
            List<PercentOfTheAmount> percentsOfTheAmount,
            double fixedPercent,
            double maxWithdrawAmount,
            double maxRemittanceAmount,
            double creditLimit,
            double comission,
            DateTime accountUnblockingPeriod)
        {
            Id = _bankIdCounter++;
            Name = bankName;
            _percentsOfTheAmount = percentsOfTheAmount;
            _clientsAccounts = new List<IPercentAccrualObserver>();
            _fixedPercent = fixedPercent;
            _maxWithdrawAmount = maxWithdrawAmount;
            _maxRemittanceAmount = maxRemittanceAmount;
            _creditLimit = creditLimit;
            _comission = comission;
            _accountUnblockingPeriod = accountUnblockingPeriod;
            _clientsAccountObservers = new List<IPercentAccrualObserver>();
        }

        public int Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<IPercentAccrualObserver> GetClientsAccountObservers()
        {
            return _clientsAccountObservers.AsReadOnly();
        }

        public ReadOnlyCollection<IPercentAccrualObserver> GetClientsAccounts()
        {
            return _clientsAccounts.AsReadOnly();
        }

        public Account CreateAccount(AccountBuilder accountBuilder, Person person, double startBalance)
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
            person.AddNewAccount(accountBuilder.Account);

            return accountBuilder.Account;
        }

        public void Replenishment(Account account, double amount)
        {
            var transaction = new TransactionReplenishment(null, account, amount, account.TransactionIdCounter++);
            account.NewTransaction(transaction);

            if (account.AccountUnblockingPeriod != DateTime.MinValue)
            {
                UpdateDepositPercent(account.Balance);
            }
        }

        public void Withdraw(Account account, double amount)
        {
            if (account.AccountUnblockingPeriod > DateTime.Now)
            {
                throw new BanksException(
                    $"Unblocking period ({account.AccountUnblockingPeriod}) did not come, you can't withdraw any money");
            }

            var transaction = new TransactionWithdraw(null, account, amount, account.TransactionIdCounter++);
            account.NewTransaction(transaction);

            if (account.AccountUnblockingPeriod != DateTime.MinValue)
            {
                UpdateDepositPercent(account.Balance);
            }
        }

        public void Remittance(Account sender, Account recipient, double amount, string message = "")
        {
            if (sender.AccountUnblockingPeriod > DateTime.Now)
            {
                throw new BanksException(
                    $"Unblocking period ({sender.AccountUnblockingPeriod}) did not come, you can't transfer any money");
            }

            var transaction = new TransactionRemittance(sender, recipient, amount, sender.TransactionIdCounter++);
            sender.NewTransaction(transaction);

            if (sender.AccountUnblockingPeriod != DateTime.MinValue)
            {
                UpdateDepositPercent(sender.Balance);
            }

            if (recipient.AccountUnblockingPeriod != DateTime.MinValue)
            {
                UpdateDepositPercent(recipient.Balance);
            }
        }

        public void Cancellation(Account account, Transaction oldTransaction)
        {
            var transaction = new TransactionCancellation(oldTransaction);
            account.NewTransaction(transaction);

            if (account.AccountUnblockingPeriod != DateTime.MinValue)
            {
                UpdateDepositPercent(account.Balance);
            }
        }

        public void Update(DateTime date)
        {
            NotifyObservers(date);
        }

        public void RegisterObserver(IPercentAccrualObserver account)
        {
            if (_clientsAccountObservers.Contains(account))
            {
                throw new BanksException("Account has already been added to observers");
            }

            _clientsAccountObservers.Add(account);
        }

        public void RemoveObserver(IPercentAccrualObserver account)
        {
            if (!_clientsAccountObservers.Contains(account))
            {
                throw new BanksException("Account has already been removed to observers");
            }

            _clientsAccountObservers.Remove(account);
        }

        public void NotifyObservers(DateTime date)
        {
            foreach (var observer in _clientsAccounts)
            {
                observer.Update(date);
            }
        }

        private double UpdateDepositPercent(double balance)
        {
            foreach (var percent in _percentsOfTheAmount
                .Where(percent => balance >= percent.LowerBound && balance <= percent.UpperBound))
            {
                return percent.Percent;
            }

            throw new BanksException("Bank error, try again later");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Banks.AccountTypes;
using Banks.BankMessages;
using Banks.Exceptions;
using Banks.Observers;

namespace Banks
{
    public class Bank : IChangesNotifyObservable
    {
        private static int _bankIdCounter = 1;

        private static long _accountIdCounter = 1234567800000000;

        private List<Account> _clientsAccounts;

        private Dictionary<Account, IChangesNotifyObserver> _clientsAccountObservers;

        private List<PercentOfTheAmount> _percentsOfTheAmount;

        private double _fixedPercent;

        private double _maxWithdrawAmount;

        private double _maxRemittanceAmount;

        private double _creditLimit;

        private double _commission;

        private DateTime _accountUnblockingPeriod;

        public Bank(
            string bankName,
            List<PercentOfTheAmount> percentsOfTheAmount,
            double fixedPercent,
            double maxWithdrawAmount,
            double maxRemittanceAmount,
            double creditLimit,
            double commission,
            DateTime accountUnblockingPeriod)
        {
            Id = _bankIdCounter++;
            Name = bankName;
            _percentsOfTheAmount = percentsOfTheAmount;
            _clientsAccountObservers = new Dictionary<Account, IChangesNotifyObserver>();
            _clientsAccounts = new List<Account>();
            _fixedPercent = fixedPercent;
            _maxWithdrawAmount = maxWithdrawAmount;
            _maxRemittanceAmount = maxRemittanceAmount;
            _creditLimit = creditLimit;
            _commission = commission;
            _accountUnblockingPeriod = accountUnblockingPeriod;
        }

        public int Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Account> GetClientsAccounts()
        {
            return _clientsAccounts.AsReadOnly();
        }

        public Dictionary<Account, IChangesNotifyObserver> GetNotifiedAccounts()
        {
            return _clientsAccountObservers;
        }

        public Account CreateCreditAccount(Person person, double startBalance)
        {
            var account = Account.CreateBuilder(_accountIdCounter++)
                .SetStartBalance(startBalance)
                .SetMaxWithdraw(_maxWithdrawAmount)
                .SetMaxRemittance(_maxRemittanceAmount)
                .SetCreditLimit(_creditLimit)
                .SetCommission(_commission)
                .Build();

            _clientsAccounts.Add(account);
            person.AddNewAccount(account);

            return account;
        }

        public Account CreateDebitAccount(Person person, double startBalance)
        {
            var account = Account.CreateBuilder(_accountIdCounter++)
                .SetPercent(_fixedPercent)
                .SetStartBalance(startBalance)
                .SetMaxWithdraw(_maxWithdrawAmount)
                .SetMaxRemittance(_maxRemittanceAmount)
                .Build();

            _clientsAccounts.Add(account);
            person.AddNewAccount(account);

            return account;
        }

        public Account CreateDepositAccount(Person person, double startBalance)
        {
            var account = Account.CreateBuilder(_accountIdCounter++)
                .SetPercent(_fixedPercent)
                .SetDepositPercent(UpdateDepositPercent(startBalance))
                .SetStartBalance(startBalance)
                .SetMaxWithdraw(_maxWithdrawAmount)
                .SetMaxRemittance(_maxRemittanceAmount)
                .SetAccountUnblockingPeriod(_accountUnblockingPeriod)
                .Build();

            _clientsAccounts.Add(account);
            person.AddNewAccount(account);

            return account;
        }

        public void SetCreditLimit(double amount)
        {
            var oldCreditLimit = _creditLimit;
            _creditLimit = amount;
            var observersList = new Dictionary<Account, IChangesNotifyObserver>();
            foreach (var account in _clientsAccounts.Where(account => account.CreditLimit != 0))
            {
                account.ReduceMoney(oldCreditLimit);
                account.IncreaseMoney(_creditLimit);
                account.CreditLimit = _creditLimit;
            }

            foreach (var observer in _clientsAccountObservers)
            {
                if (observer.Key.CreditLimit != 0)
                {
                    observersList.Add(observer.Key, observer.Value);
                }
            }

            IBankMessage message = new CreditLimitChangeMsg();
            NotifyObservers(observersList, _creditLimit, message);
        }

        public void SetFixedPercent(double amount)
        {
            _fixedPercent = amount;
            var observersList = new Dictionary<Account, IChangesNotifyObserver>();
            foreach (var account in _clientsAccounts.Where(account => account.Percent != 0))
            {
                account.Percent = _fixedPercent;
            }

            foreach (var observer in _clientsAccountObservers)
            {
                if (observer.Key.Percent != 0)
                {
                    observersList.Add(observer.Key, observer.Value);
                }
            }

            IBankMessage message = new CreditLimitChangeMsg();
            NotifyObservers(observersList, _fixedPercent, message);
        }

        public void SetMaxRemittanceAmount(double amount)
        {
            _maxRemittanceAmount = amount;
            IBankMessage message = new CreditLimitChangeMsg();
            foreach (var account in _clientsAccounts)
            {
                account.MaxRemittance = _maxRemittanceAmount;
            }

            NotifyObservers(_clientsAccountObservers, _maxRemittanceAmount, message);
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

        public void BalanceUpdate(DateTime date)
        {
            foreach (var observer in _clientsAccounts)
            {
                observer.AnyBalanceTimeChange(date);
            }
        }

        public void RegisterObserver(Account account, IChangesNotifyObserver accountsObserver)
        {
            if (_clientsAccountObservers.Any(accountObserver =>
                accountObserver.Value == accountsObserver))
            {
                throw new BanksException("Account has already been added to notified accounts list");
            }

            _clientsAccountObservers.Add(account, accountsObserver);
        }

        public void RemoveObserver(Account account, IChangesNotifyObserver accountsObserver)
        {
            if (_clientsAccountObservers.Any(accountObserver =>
                accountObserver.Value == accountsObserver))
            {
                _clientsAccountObservers.Remove(account);
                return;
            }

            throw new BanksException("Account has already been added to notified accounts list");
        }

        public void NotifyObservers(Dictionary<Account, IChangesNotifyObserver> observers, double amount, IBankMessage message)
        {
            foreach (var observer in observers)
            {
                observer.Value.Update(message.MessageToClient(observer.Key, amount));
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
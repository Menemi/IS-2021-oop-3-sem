using System;
using System.Collections.Generic;
using Banks.Exceptions;
using Banks.Observers;

namespace Banks
{
    public class CentralBank : IObservable
    {
        private static int _idCounter = 1;

        private List<IObserver> _banks;

        public CentralBank(string cBankName)
        {
            Id = _idCounter++;
            Name = cBankName;
            _banks = new List<IObserver>();
        }

        public int Id { get; }

        public string Name { get; }

        public Bank CreateBank(
            string bankName,
            List<PercentOfTheAmount> percentsOfTheAmount,
            float fixedPercent,
            float maxWithdrawAmount,
            float maxRemittanceAmount,
            float creditLimit,
            float comission,
            DateTime accountUnblockingPeriod)
        {
            var bank = new Bank(
                bankName,
                percentsOfTheAmount,
                fixedPercent,
                maxWithdrawAmount,
                maxRemittanceAmount,
                creditLimit,
                comission,
                accountUnblockingPeriod);
            RegisterObserver(bank);
            return bank;
        }

        public void RegisterObserver(IObserver bank)
        {
            if (!_banks.Contains(bank))
            {
                throw new BanksException("Account has already been removed to observers");
            }

            _banks.Add(bank);
        }

        public void RemoveObserver(IObserver bank)
        {
            if (!_banks.Contains(bank))
            {
                throw new BanksException("Account has already been removed to observers");
            }

            _banks.Remove(bank);
        }

        public void NotifyObservers(DateTime date)
        {
            foreach (var observer in _banks)
            {
                observer.Update(date);
            }
        }
    }
}
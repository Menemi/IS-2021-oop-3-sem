using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Banks.AccountTypes;
using Banks.Exceptions;
using Banks.Observers;

namespace Banks
{
    public class CentralBank
    {
        private static int _idCounter = 1;

        private List<Bank> _banks;

        public CentralBank(string cBankName)
        {
            Id = _idCounter++;
            Name = cBankName;
            _banks = new List<Bank>();
        }

        public int Id { get; }

        public string Name { get; }

        public Bank CreateBank(
            string bankName,
            List<PercentOfTheAmount> percentsOfTheAmount,
            double fixedPercent,
            double maxWithdrawAmount,
            double maxRemittanceAmount,
            double creditLimit,
            double commission,
            DateTime accountUnblockingPeriod)
        {
            var bank = new Bank(
                bankName,
                percentsOfTheAmount,
                fixedPercent,
                maxWithdrawAmount,
                maxRemittanceAmount,
                creditLimit,
                commission,
                accountUnblockingPeriod);
            _banks.Add(bank);
            return bank;
        }

        public ReadOnlyCollection<Bank> GetBanks()
        {
            return _banks.AsReadOnly();
        }

        public void NotifyObservers(DateTime date)
        {
            foreach (var bank in _banks)
            {
                bank.BalanceUpdate(date);
            }
        }
    }
}
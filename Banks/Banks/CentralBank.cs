using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Banks
{
    public class CentralBank
    {
        private static CentralBank instance;

        private List<Bank> _banks;

        protected CentralBank(string cBankName)
        {
            Name = cBankName;
            _banks = new List<Bank>();
        }

        public string Name { get; }

        public static CentralBank GetInstance(string name)
        {
            return instance ?? (instance = new CentralBank(name));
        }

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
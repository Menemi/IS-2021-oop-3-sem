using System;
using System.Collections.Generic;

namespace Banks
{
    public class CentralBank
    {
        private static int _idCounter = 1;

        private List<Bank> _banks;

        // Регистрацией всех банков, а также взаимодействием между банками занимается центральный банк. Он должен
        // управлять банками (предоставлять возможность создать банк) и предоставлять необходимый функционал, чтобы
        // банки могли взаимодействовать с другими банками (например, можно реализовать переводы между банками через
        // него). Он также занимается уведомлением других банков о том, что нужно начислять остаток или комиссию - для
        // этого механизма не требуется создавать таймеры и завязываться на реальное время.
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
            _banks.Add(bank);
            return bank;
        }
    }
}
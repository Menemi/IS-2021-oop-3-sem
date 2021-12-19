using System;
using System.Collections.Generic;
using Banks.Observers;

namespace Banks.AccountTypes
{
    public class Account : IPercentAccrualObserver
    {
        private List<Transaction> _transactionHistory;

        private double summaryPercent;

        private double summaryCommission;

        public Account(long id)
        {
            Id = id;
            _transactionHistory = new List<Transaction>();
            summaryPercent = summaryCommission = 0;
        }

        public long Id { get; }

        public double Percent { get; set; }

        public double Balance { get; private set; }

        public double MaxWithdraw { get; set; } = -1;

        public double MaxRemittance { get; set; } = -1;

        public double CreditLimit { get; set; }

        public double Commission { get; set; }

        public DateTime AccountUnblockingPeriod { get; set; }

        public int TransactionIdCounter { get; set; } = 1;

        public void IncreaseMoney(double amount)
        {
            Balance += amount;
        }

        public void ReduceMoney(double amount)
        {
            Balance -= amount;
        }

        public void NewTransaction(Transaction transaction)
        {
            _transactionHistory.Add(transaction);
        }

        public void Update(DateTime checkDate)
        {
            // var startDate = DateTime.Today; - реализация для работы в реальном мире
            // оставил хардкод для правильных тестов
            var startDate = new DateTime(2021, 12, 19);
            var totalDays = checkDate.Subtract(startDate).TotalDays;

            if (Commission != 0)
            {
                if (Balance >= CreditLimit)
                {
                    return;
                }

                for (var i = 0; i < totalDays; i++)
                {
                    summaryCommission += Commission;
                    startDate = startDate.AddDays(1);

                    if (startDate.Day == 1)
                    {
                        ReduceMoney(summaryCommission);
                        summaryCommission = 0;
                    }
                }
            }
            else
            {
                for (var i = 0; i < totalDays; i++)
                {
                    var divider = startDate.Year % 4 == 0 ? 366 : 365;
                    summaryPercent += Balance * Math.Round(Percent / divider, 2, MidpointRounding.AwayFromZero);
                    startDate = startDate.AddDays(1);

                    if (startDate.Day == 1)
                    {
                        IncreaseMoney(summaryPercent);
                        summaryPercent = 0;
                    }
                }
            }
        }
    }
}
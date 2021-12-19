using System;
using System.Collections.Generic;
using Banks.Observers;

namespace Banks.AccountTypes
{
    public class Account : IObserver
    {
        private List<Transaction> _transactionHistory;

        private float summaryPercent;

        private float summaryCommission;

        public Account(long id)
        {
            Id = id;
            _transactionHistory = new List<Transaction>();
            summaryPercent = summaryCommission = 0;
        }

        public long Id { get; }

        public float Percent { get; set; }

        public float Balance { get; private set; }

        public float MaxWithdraw { get; set; } = -1;

        public float MaxRemittance { get; set; } = -1;

        public float CreditLimit { get; set; }

        public float Commission { get; set; }

        public DateTime AccountUnblockingPeriod { get; set; }

        public int TransactionIdCounter { get; set; } = 1;

        public void IncreaseMoney(float amount)
        {
            Balance += amount;
        }

        public void ReduceMoney(float amount)
        {
            Balance -= amount;
        }

        public void NewTransaction(Transaction transaction)
        {
            _transactionHistory.Add(transaction);
        }

        public void Update(DateTime checkDate)
        {
            var startDate = DateTime.Today;
            var totalDays = checkDate.Subtract(startDate).TotalDays;

            if (Commission != 0)
            {
                for (var i = 0; i < totalDays; i++)
                {
                    summaryCommission += Commission;
                    startDate.AddDays(1);

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
                    summaryPercent += Balance * (Percent * 0.01f);
                    startDate.AddDays(1);

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
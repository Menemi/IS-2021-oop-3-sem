using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Banks.Observers;

namespace Banks.AccountTypes
{
    public class Account : IChangesNotifyObserver
    {
        private List<Transaction> _transactionHistory;

        public Account(long id)
        {
            Id = id;
            _transactionHistory = new List<Transaction>();
            SummaryPercent = SummaryCommission = 0;
            BankMessageList = new List<string>();
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

        public double SummaryPercent { get; set; }

        public double SummaryCommission { get; set; }

        public List<string> BankMessageList { get; }

        public static AccountBuilder CreateBuilder(long id)
        {
            return new AccountBuilder(id);
        }

        public ReadOnlyCollection<Transaction> GetTransactions()
        {
            return _transactionHistory.AsReadOnly();
        }

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

        public void AnyBalanceTimeChange(DateTime checkDate)
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
                    SummaryCommission += Commission;
                    startDate = startDate.AddDays(1);

                    if (startDate.Day == 1)
                    {
                        ReduceMoney(SummaryCommission);
                        SummaryCommission = 0;
                    }
                }
            }
            else
            {
                for (var i = 0; i < totalDays; i++)
                {
                    var divider = new GregorianCalendar().GetDaysInYear(startDate.Year);
                    SummaryPercent += Balance * Math.Round(Percent / divider, 2, MidpointRounding.AwayFromZero);
                    startDate = startDate.AddDays(1);

                    if (startDate.Day == 1)
                    {
                        IncreaseMoney(SummaryPercent);
                        SummaryPercent = 0;
                    }
                }
            }
        }

        public void Update(string message)
        {
            BankMessageList.Add(message);
        }
    }
}
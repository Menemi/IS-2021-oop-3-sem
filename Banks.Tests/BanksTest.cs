using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountTypes;
using Banks.Exceptions;
using NUnit.Framework;

namespace Banks.Tests
{
    public class Tests
    {
        private TimeMachine _timeMachine;
        private CentralBank _centralBank;

        [SetUp]
        public void Setup()
        {
            _centralBank = new CentralBank("Central Bank");
            _timeMachine = new TimeMachine();
        }

        [Test]
        public void WithdrawAndRemmitanceLimitTest()
        {
            var depositPercentsList = new List<PercentOfTheAmount>()
            {
                new PercentOfTheAmount(0, 50000, 3),
                new PercentOfTheAmount(50000.01, 100000, 6),
                new PercentOfTheAmount(100000.01, float.MaxValue, 9),
            };

            var tinkoff = _centralBank.CreateBank(
                "Tinkoff",
                depositPercentsList,
                3,
                10000,
                10000,
                10000,
                1000,
                new DateTime(2022, 1, 1));

            AccountBuilder accountBuilder = new DebitAccount();
            ClientBuilder clientBuilder = new Client();

            var putin = clientBuilder.CreateNewClient("Vladimir", "Putin");
            clientBuilder.SetAddress("ул. Ильинка, д. 23, 103132, Москва, Россия");
            clientBuilder.SetPassport(new Passport(7777, 777777));
            clientBuilder = new Client();
            var biden = clientBuilder.CreateNewClient("Joe", "Biden");
            clientBuilder.SetPassport(new Passport(6666, 666666));

            var putinDebit = tinkoff.CreateAccount(accountBuilder, putin, 100000);
            var bidenDebit = tinkoff.CreateAccount(accountBuilder, biden, 200000);

            tinkoff.Remittance(bidenDebit, putinDebit, 100, "лови сотку, задолжал же тебе...");
            Assert.AreEqual(putinDebit.Balance, 100100);
            Assert.Throws<BanksException>(() => tinkoff.Withdraw(bidenDebit, 30000));
            Assert.Throws<BanksException>(() => tinkoff.Remittance(bidenDebit, putinDebit, 30000));
        }

        [Test]
        public void TimeMachineTest()
        {
            var depositPercentsList = new List<PercentOfTheAmount>()
            {
                new PercentOfTheAmount(0, 50000, 3),
                new PercentOfTheAmount(50000.01, 100000, 6),
                new PercentOfTheAmount(100000.01, float.MaxValue, 9),
            };

            var tinkoff = _centralBank.CreateBank(
                "Tinkoff",
                depositPercentsList,
                3,
                10000,
                10000,
                10000,
                1000,
                new DateTime(2022, 1, 1));

            AccountBuilder accountBuilder = new DebitAccount();
            ClientBuilder clientBuilder = new Client();

            var putin = clientBuilder.CreateNewClient("Vladimir", "Putin");
            clientBuilder.SetAddress("ул. Ильинка, д. 23, 103132, Москва, Россия");
            clientBuilder.SetPassport(new Passport(7777, 777777));
            clientBuilder = new Client();
            var biden = clientBuilder.CreateNewClient("Joe", "Biden");
            clientBuilder.SetPassport(new Passport(6666, 666666));

            var putinDebit = tinkoff.CreateAccount(accountBuilder, putin, 100000);
            var bidenDebit = tinkoff.CreateAccount(accountBuilder, biden, 200000);

            accountBuilder = new CreditAccount();
            var putinCredit = tinkoff.CreateAccount(accountBuilder, putin, 100000);

            accountBuilder = new DepositAccount();
            var putinDeposit = tinkoff.CreateAccount(accountBuilder, putin, 100000);
            var bidenDeposit = tinkoff.CreateAccount(accountBuilder, biden, 10000);

            var dateToRewind = new DateTime(2022, 1, 1);
            _timeMachine.TimeRewind(_centralBank, dateToRewind);

            Assert.AreEqual(putin.GetAccounts().Count, 3);
            Assert.AreEqual(putinDebit.Balance, 113000);
            Assert.AreEqual(bidenDebit.Balance, 226000);
            Assert.AreEqual(putinCredit.Balance, 110000);
            Assert.AreEqual(putinDeposit.Balance, 126000);
            Assert.AreEqual(bidenDeposit.Balance, 11300);
        }

        [Test]
        public void NotifiesTest()
        {
            var depositPercentsList = new List<PercentOfTheAmount>()
            {
                new PercentOfTheAmount(0, 50000, 3),
                new PercentOfTheAmount(50000.01, 100000, 6),
                new PercentOfTheAmount(100000.01, float.MaxValue, 9),
            };

            var tinkoff = _centralBank.CreateBank(
                "Tinkoff",
                depositPercentsList,
                3,
                10000,
                10000,
                10000,
                1000,
                new DateTime(2022, 1, 1));

            AccountBuilder accountBuilder = new DebitAccount();
            ClientBuilder clientBuilder = new Client();

            var putin = clientBuilder.CreateNewClient("Vladimir", "Putin");
            clientBuilder.SetAddress("ул. Ильинка, д. 23, 103132, Москва, Россия");
            clientBuilder.SetPassport(new Passport(7777, 777777));
            clientBuilder = new Client();
            var biden = clientBuilder.CreateNewClient("Joe", "Biden");
            clientBuilder.SetPassport(new Passport(6666, 666666));

            var putinDebit = tinkoff.CreateAccount(accountBuilder, putin, 100000);
            var bidenDebit = tinkoff.CreateAccount(accountBuilder, biden, 200000);

            accountBuilder = new CreditAccount();
            var putinCredit = tinkoff.CreateAccount(accountBuilder, putin, 100000);

            accountBuilder = new DepositAccount();
            var putinDeposit = tinkoff.CreateAccount(accountBuilder, putin, 100000);
            var bidenDeposit = tinkoff.CreateAccount(accountBuilder, biden, 10000);

            tinkoff.RegisterObserver(putinDebit, putinDebit);
            tinkoff.RegisterObserver(putinCredit, putinCredit);
            tinkoff.RegisterObserver(bidenDeposit, bidenDeposit);

            tinkoff.SetFixedPercent(3.5);
            tinkoff.SetCreditLimit(11000);
            tinkoff.SetMaxRemittanceAmount(11000);

            Assert.AreEqual(tinkoff.GetClientsAccounts().Count, 5);
            Assert.AreEqual(tinkoff.GetNotifiedAccounts().Count, 3);
            Assert.AreEqual(putinDebit.Percent, 3.5);
            Assert.AreEqual(putinDebit.MaxRemittance, 11000);
            Assert.AreEqual(putinDebit.BankMessageList.Count, 2);
            Assert.AreEqual(bidenDebit.Percent, 3.5);
            Assert.AreEqual(bidenDebit.BankMessageList.Count, 0);
        }
    }
}
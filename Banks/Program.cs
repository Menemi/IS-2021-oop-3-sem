using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.AccountTypes;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            string chooseTypeOfUser;
            var timeMachine = new TimeMachine();
            var centralBank = new CentralBank("Central Bank");
            var people = new List<Person>();

            // краткий гайд по работе с тестами: когда предлагаются на выбор функции, всегда можно писать либо номер
            // функции, либо её название. В примере я оставил именно названия функций, чтоб можно было легко
            // ориентироваться при проверке
            // также я оставил комментарии в виде туду, расставив их перед каждой функцией с соответсвующим названием,
            // так что при проверке тоже можно легко и быстро свапаться между функциями
            void ChoosingTypeOfUser()
            {
                Console.WriteLine("Type your user type 'bank manager / client' (you can type 'choose again' to " +
                                  "choose again or 'stop' to end the program):");
                chooseTypeOfUser = Console.ReadLine();
                Console.WriteLine();
                UsersTypeLogic();
                return;
            }

            // да, я понимаю, что свитч-кейс - круто, но пощадите, пожалуйста...
            // я правда знаю, как с ним работать
            void BankManager()
            {
                while (true)
                {
                    // todo: BANK MANAGER
                    Console.WriteLine("You can type:");
                    Console.WriteLine("1. Create bank");
                    Console.WriteLine("2. Banks list");
                    Console.WriteLine("3. Rewind time");
                    Console.WriteLine("4. Choose again (send you to start menu)");
                    var function = Console.ReadLine();

                    // todo: create bank
                    if (function.ToLower() == "create bank" || function == "1")
                    {
                        Console.WriteLine("Type all the parametrs with Enter:");
                        Console.WriteLine("1. bank name");
                        var bankName = Console.ReadLine();

                        Console.WriteLine(
                            "2. percents for deposit account (count, lower bound, upper bound, percent (count times))");
                        var percentsOfTheAmount = new List<PercentOfTheAmount>();
                        var times = Convert.ToInt32(Console.ReadLine());
                        for (var i = 0; i < times; i++)
                        {
                            percentsOfTheAmount.Add(new PercentOfTheAmount(
                                Convert.ToDouble(Console.ReadLine()),
                                Convert.ToDouble(Console.ReadLine()),
                                Convert.ToDouble(Console.ReadLine())));
                        }

                        Console.WriteLine("3. fixed percent");
                        var fixedPercent = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("4. max withdraw amount");
                        var maxWithdrawAmount = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("5. max remittance amount");
                        var maxRemittanceAmount = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("6. credit limit");
                        var creditLimit = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("7. commission");
                        var commission = Convert.ToDouble(Console.ReadLine());

                        Console.WriteLine("8. account unblocking period (year, month, day)");
                        var accountUnblockingPeriod = new DateTime(
                            Convert.ToInt32(Console.ReadLine()),
                            Convert.ToInt32(Console.ReadLine()),
                            Convert.ToInt32(Console.ReadLine()));

                        centralBank.CreateBank(
                            bankName,
                            percentsOfTheAmount,
                            fixedPercent,
                            maxWithdrawAmount,
                            maxRemittanceAmount,
                            creditLimit,
                            commission,
                            accountUnblockingPeriod);

                        Console.WriteLine();
                    }

                    // todo: banks list
                    else if (function.ToLower() == "banks list" || function == "2")
                    {
                        foreach (var bank in centralBank.GetBanks())
                        {
                            Console.WriteLine($"{bank.Id}. {bank.Name} (id: {bank.Id})");
                        }

                        Console.WriteLine();
                    }

                    // todo: rewind time
                    else if (function.ToLower() == "rewind time" || function == "3")
                    {
                        Console.WriteLine("Type date to rewind (year, month, day)");
                        var dateToRewind = new DateTime(
                            Convert.ToInt32(Console.ReadLine()),
                            Convert.ToInt32(Console.ReadLine()),
                            Convert.ToInt32(Console.ReadLine()));
                        timeMachine.TimeRewind(centralBank, dateToRewind);

                        ChoosingTypeOfUser();
                        return;
                    }

                    // todo: choose again
                    else if (function.ToLower() == "choose again" || function == "4")
                    {
                        Console.WriteLine();
                        ChoosingTypeOfUser();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            void Client()
            {
                while (true)
                {
                    // todo: CLIENT
                    Console.WriteLine("You can type:");
                    Console.WriteLine("1. Register in system");
                    Console.WriteLine("2. Create new account");
                    Console.WriteLine("3. Accounts list");
                    Console.WriteLine("4. Balance");
                    Console.WriteLine("5. Withdraw");
                    Console.WriteLine("6. Replenishment");
                    Console.WriteLine("7. Remmitance");
                    Console.WriteLine("8. Cancel transaction");
                    Console.WriteLine("9. Transaction history");
                    Console.WriteLine("10. Choose again");

                    var function = Console.ReadLine();

                    // todo: register in system
                    if (function.ToLower() == "register in system" || function == "1")
                    {
                        Console.WriteLine("Type you name and surname:");
                        ClientBuilder clientBuilder = new Client();
                        var person = clientBuilder.CreateNewClient(Console.ReadLine(), Console.ReadLine());

                        Console.WriteLine(
                            "Type your address if you want to verify your profile or just tap Enter");

                        var address = Console.ReadLine();
                        if (address != string.Empty)
                        {
                            clientBuilder.SetAddress("str");
                        }

                        Console.WriteLine(
                            "Type your passport (series ENTER number) if you want to verify your profile or just tap Enter");
                        var series = Console.ReadLine();
                        var number = Console.ReadLine();
                        if (series != string.Empty && number != string.Empty)
                        {
                            clientBuilder.SetPassport(new Passport(Convert.ToInt32(series), Convert.ToInt32(number)));
                        }

                        people.Add(person);

                        Console.WriteLine();
                    }

                    // todo: create new account
                    else if (function.ToLower() == "create new account" || function == "2")
                    {
                        Console.WriteLine("Choose the bank (type the number):");
                        foreach (var bank in centralBank.GetBanks())
                        {
                            Console.WriteLine($"{bank.Id}. {bank.Name} (id: {bank.Id})");
                        }

                        var bankNumber = Convert.ToInt32(Console.ReadLine());
                        foreach (var bank in centralBank.GetBanks())
                        {
                            if (bank.Id == bankNumber)
                            {
                                Console.WriteLine("Type your full name:");
                                var name = Console.ReadLine();
                                foreach (var person in people)
                                {
                                    if (person.Name + " " + person.Surname == name)
                                    {
                                        Console.WriteLine(
                                            "Type the type of your account (credit / debit / deposit) and " +
                                            "then make a deposit (or type 0):");
                                        var accountType = Console.ReadLine();
                                        if (accountType == "credit")
                                        {
                                            AccountBuilder accountBuilder = new CreditAccount();
                                            var newCredit = bank.CreateAccount(
                                                accountBuilder, person, Convert.ToDouble(Console.ReadLine()));
                                        }
                                        else if (accountType == "debit")
                                        {
                                            AccountBuilder accountBuilder = new DebitAccount();
                                            var newDebit = bank.CreateAccount(
                                                accountBuilder, person, Convert.ToDouble(Console.ReadLine()));
                                        }
                                        else if (accountType == "deposit")
                                        {
                                            AccountBuilder accountBuilder = new DepositAccount();
                                            var newDeposit = bank.CreateAccount(
                                                accountBuilder, person, Convert.ToDouble(Console.ReadLine()));
                                        }
                                    }

                                    break;
                                }

                                break;
                            }
                        }

                        Console.WriteLine();
                    }

                    // todo: accounts list
                    else if (function.ToLower() == "accounts list" || function == "3")
                    {
                        Console.WriteLine("Type your full name:");
                        var name = Console.ReadLine();
                        foreach (var person in people)
                        {
                            if (person.Name + " " + person.Surname == name)
                            {
                                var tempCounter = 1;
                                foreach (var account in person.GetAccounts())
                                {
                                    // естесвенно я понимаю, что можно вывести больше информации,
                                    // просто это займёт больше аналогичных строк
                                    Console.WriteLine($"{tempCounter++}. id: {account.Id}; balance: {account.Balance}");
                                }
                            }
                        }

                        Console.WriteLine();
                    }

                    // todo: balance
                    else if (function.ToLower() == "balance" || function == "4")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var account in person.GetAccounts())
                            {
                                if (account.Id.ToString() == id)
                                {
                                    Console.WriteLine($"Your balance: {account.Balance}");
                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: withdraw
                    else if (function.ToLower() == "withdraw" || function == "5")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var account in person.GetAccounts())
                            {
                                if (account.Id.ToString() == id)
                                {
                                    foreach (var bank in centralBank.GetBanks())
                                    {
                                        if (bank.GetClientsAccounts().Contains(account))
                                        {
                                            Console.WriteLine("Type how much money do you want to withdraw:");
                                            bank.Withdraw(account, Convert.ToDouble(Console.ReadLine()));
                                            Console.WriteLine($"Now your balance on this account: {account.Balance}");
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: replenishment
                    else if (function.ToLower() == "replenishment" || function == "6")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var account in person.GetAccounts())
                            {
                                if (account.Id.ToString() == id)
                                {
                                    foreach (var bank in centralBank.GetBanks())
                                    {
                                        if (bank.GetClientsAccounts().Contains(account))
                                        {
                                            Console.WriteLine("Type how much money do you want to replenish:");
                                            bank.Replenishment(account, Convert.ToDouble(Console.ReadLine()));
                                            Console.WriteLine($"Now your balance on this account: {account.Balance}");
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: remmitance
                    else if (function.ToLower() == "remmitance" || function == "7")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var sender in person.GetAccounts())
                            {
                                if (sender.Id.ToString() == id)
                                {
                                    foreach (var bank in centralBank.GetBanks())
                                    {
                                        if (bank.GetClientsAccounts().Contains(sender))
                                        {
                                            Console.WriteLine("Type the account id to which you want to transfer");
                                            var recipientAccountId = Console.ReadLine();
                                            foreach (var bank2 in centralBank.GetBanks())
                                            {
                                                foreach (var recipient in bank2.GetClientsAccounts())
                                                {
                                                    if (recipient.Id.ToString() == recipientAccountId)
                                                    {
                                                        Console.WriteLine(
                                                            "Type how much money do you want to transfer:");
                                                        var amount = Convert.ToDouble(Console.ReadLine());
                                                        bank.Remittance(
                                                            sender,
                                                            recipient,
                                                            amount);
                                                        Console.WriteLine(
                                                            $"Now your balance on this account: {sender.Balance}");
                                                        Console.WriteLine(
                                                            $"And your recipient balance now is: {recipient.Balance}");
                                                        break;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: cancel transaction
                    else if (function.ToLower() == "cancel transaction" || function == "8")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var account in person.GetAccounts())
                            {
                                if (account.Id.ToString() == id)
                                {
                                    foreach (var bank in centralBank.GetBanks())
                                    {
                                        if (bank.GetClientsAccounts().Contains(account))
                                        {
                                            Console.WriteLine("Type the transaction id:");
                                            foreach (var transaction in account.GetTransactions())
                                            {
                                                if (transaction.Id.ToString() == Console.ReadLine())
                                                {
                                                    bank.Cancellation(account, transaction);
                                                    Console.WriteLine(
                                                        $"Now your balance on this account: {account.Balance}");
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: transaction history
                    else if (function.ToLower() == "transaction history" || function == "9")
                    {
                        Console.WriteLine("Type your full name and then your account id:");
                        var name = Console.ReadLine();
                        var id = Console.ReadLine();
                        foreach (var person in people.Where(person => person.Name + " " + person.Surname == name))
                        {
                            foreach (var account in person.GetAccounts())
                            {
                                if (account.Id.ToString() == id)
                                {
                                    if (centralBank.GetBanks().Any(bank => bank.GetClientsAccounts().Contains(account)))
                                    {
                                        foreach (var transaction in account.GetTransactions())
                                        {
                                            Console.WriteLine($"Transaction id: {transaction.Id}");
                                            Console.WriteLine($"Recipient id: {transaction.Recipient.Id}");
                                            if (transaction.Sender != null)
                                            {
                                                Console.WriteLine($"Sender id: {transaction.Sender.Id}");
                                            }

                                            Console.WriteLine(
                                                $"Transaction amount: {transaction.TransactionAmount}");
                                            Console.WriteLine(
                                                $"Transaction time: {transaction.TransactionTime.ToString()}");
                                            Console.WriteLine();
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }

                        Console.WriteLine();
                    }

                    // todo: choose again
                    else if (function.ToLower() == "choose again" || function == "10")
                    {
                        Console.WriteLine();
                        ChoosingTypeOfUser();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            void UsersTypeLogic()
            {
                if (chooseTypeOfUser.ToLower() == "bank manager" || chooseTypeOfUser == "1")
                {
                    BankManager();
                }
                else if (chooseTypeOfUser.ToLower() == "client" || chooseTypeOfUser == "2")
                {
                    Client();
                }
                else if (chooseTypeOfUser.ToLower() == "choose again" || chooseTypeOfUser == "3")
                {
                    ChoosingTypeOfUser();
                    return;
                }
                else if (chooseTypeOfUser.ToLower() == "stop" || chooseTypeOfUser == "4")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Wrong command, try again!");
                    ChoosingTypeOfUser();
                    return;
                }
            }

            ChoosingTypeOfUser();
        }
    }
}
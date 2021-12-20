using Banks.AccountTypes;

namespace Banks.BankMessages
{
    public class CreditLimitChangeMsg : IBankMessage
    {
        public string MessageToClient(Account account, double amount)
        {
            return new string($"Your credit limit on account {account.Id} was changed to {amount}");
        }
    }
}
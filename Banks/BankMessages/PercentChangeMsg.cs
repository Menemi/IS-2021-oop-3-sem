using Banks.AccountTypes;

namespace Banks.BankMessages
{
    public class PercentChangeMsg : IBankMessage
    {
        public string MessageToClient(Account account, double amount)
        {
            return new string($"Your percents on account {account.Id} was changed to {amount}%");
        }
    }
}
using Banks.AccountTypes;

namespace Banks.BankMessages
{
    public class RemittanceLimitChangeMsg : IBankMessage
    {
        public string MessageToClient(Account account, double amount)
        {
            return new string($"Your remittance limit on account {account.Id} was changed to {amount}. " +
                              "If you are the verified user, you can skip this message");
        }
    }
}
using Banks.AccountTypes;

namespace Banks.BankMessages
{
    public interface IBankMessage
    {
        public string MessageToClient(Account account, double amount);
    }
}
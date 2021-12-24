using System.Collections.Generic;
using Banks.AccountTypes;
using Banks.BankMessages;

namespace Banks.Observers
{
    public interface IChangesNotifyObservable
    {
        void RegisterObserver(Account account, IChangesNotifyObserver accountsObserver);

        void RemoveObserver(Account account, IChangesNotifyObserver accountsObserver);

        void NotifyObservers(Dictionary<Account, IChangesNotifyObserver> observers, double amount, IBankMessage message);
    }
}
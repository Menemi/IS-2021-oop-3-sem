using System;
using Banks.BankMessages;

namespace Banks.Observers
{
    public interface IChangesNotifyObserver
    {
        void Update(string message);
    }
}
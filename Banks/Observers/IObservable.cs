using System;

namespace Banks.Observers
{
    public interface IObservable
    {
        void RegisterObserver(IObserver obj);

        void RemoveObserver(IObserver obj);

        void NotifyObservers(DateTime date);
    }
}
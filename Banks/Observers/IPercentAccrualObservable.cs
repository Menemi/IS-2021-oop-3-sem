using System;

namespace Banks.Observers
{
    public interface IPercentAccrualObservable
    {
        void RegisterObserver(IPercentAccrualObserver obj);

        void RemoveObserver(IPercentAccrualObserver obj);

        void NotifyObservers(DateTime date);
    }
}
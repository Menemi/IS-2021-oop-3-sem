using System;

namespace Banks.Observers
{
    public interface IObserver
    {
        void Update(DateTime date);
    }
}
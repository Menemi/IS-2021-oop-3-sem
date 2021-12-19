using System;

namespace Banks.Observers
{
    public interface IPercentAccrualObserver
    {
        void Update(DateTime date);
    }
}
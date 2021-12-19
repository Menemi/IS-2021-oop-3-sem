using System;

namespace Banks
{
    public class TimeMachine
    {
        public void TimeRewind(CentralBank centralBank, DateTime dateToRewind)
        {
            centralBank.NotifyObservers(dateToRewind);
        }
    }
}
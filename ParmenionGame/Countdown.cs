using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class Countdown
    {
        private int countdownTime;
        private readonly Action<int> countdownProgressAction;
        private readonly Action countdownFinishedAction;
        private readonly Timer timer;

        public Countdown(int totalTime, Action<int> countdownProgressAction, Action countdownFinishedAction)
        {
            this.countdownTime = totalTime;
            this.countdownProgressAction = countdownProgressAction;
            this.countdownFinishedAction = countdownFinishedAction;
            this.timer = new Timer(DoCountdown, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }    

        private void DoCountdown(object? state)
        {
            countdownTime--;
            if (countdownTime <= 0)
            {
                timer.Change(Timeout.Infinite, 0);
                countdownFinishedAction();
            }
            countdownProgressAction(countdownTime);
        }
    }
}

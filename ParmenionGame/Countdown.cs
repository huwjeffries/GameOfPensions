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
        private readonly Func<int, Task> countdownProgressAction;
        private readonly Func<Task> countdownFinishedAction;
        private Timer timer;

        public Countdown(int totalTime, Func<int, Task> countdownProgressAction, Func<Task> countdownFinishedAction)
        {
            this.countdownTime = totalTime;
            this.countdownProgressAction = countdownProgressAction;
            this.countdownFinishedAction = countdownFinishedAction;
            this.timer = new Timer(DoCountdown, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }    

        private async void DoCountdown(object state)
        {
            countdownTime--;
            if (countdownTime <= 0)
            {
                StopTimer();
                await countdownFinishedAction();
            }
            await countdownProgressAction(countdownTime);
        }

        internal void StopTimer()
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
                timer = null;
            }
        }
    }
}

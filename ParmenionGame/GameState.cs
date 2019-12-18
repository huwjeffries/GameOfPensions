using System;
using System.Threading;

namespace ParmenionGame
{
    public class GameState
    {
        private Timer _timer;
        private int countdownTime = 0;

        public GameState()
        {
        }

        public void JoinGame(string code, Action<int> joinGameAction)
        {
            this.Countdown(10, joinGameAction);
        }

        private void Countdown(int totalTime, Action<int> joinGameAction)
        {
            countdownTime = totalTime;
            _timer = new Timer(DoCountdown, joinGameAction, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void DoCountdown(object countdownCallback)
        {
            countdownTime--;
            if (countdownTime <= 0)
            {
                _timer.Change(Timeout.Infinite, 0);
            }
            var callback = (Action<int>)countdownCallback;
            callback(countdownTime);
        }
    }
}

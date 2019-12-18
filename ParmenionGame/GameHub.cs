using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class GameHub : Hub
    {
        private readonly GameState state;

        public GameHub(GameState state)
        {
            this.state = state;
        }

        public async Task RegisterDashboard()
        {
            await Clients.Caller.SendAsync("JoinGameCode", "DEF");
            this.state.JoinGameCountdown("DEF", 
                (int timeRemaining) => Clients.Caller.SendAsync("JoinGameCountdown", timeRemaining), //And question countdown. Rename to dashboard countdown?
                (Question question) => Clients.Caller.SendAsync("ShowQuestion", question.QuestionText));
        }
    }
}

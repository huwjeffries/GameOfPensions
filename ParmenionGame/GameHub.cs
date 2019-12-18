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
                this.Context.ConnectionId,
                (int timeRemaining) => Clients.Caller.SendAsync("JoinGameCountdown", timeRemaining), //And question countdown. Rename to dashboard countdown?
                (Question question) => Clients.Caller.SendAsync("ShowQuestion", question.QuestionText));
        }

        public async Task JoinGame(string code, string name)
        {
            this.state.JoinGame(code, name, this.Context.ConnectionId, (string dashboardId, IEnumerable<string> playerNames) => Clients.Client(dashboardId).SendAsync("UpdatePlayerList", playerNames));
        }
    }
}

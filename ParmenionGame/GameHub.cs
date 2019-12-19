using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class GameHub : Hub<IGameHub>
    {
        private readonly GameState state;

        public GameHub(GameState state)
        {
            this.state = state;
        }

        public async Task RegisterDashboard()
        {
            await Clients.Caller.JoinGameCode("DEF");
            this.state.RegisterDashboard("DEF",
                this.Context.ConnectionId,
                (int timeRemaining) => Clients.Caller.JoinGameCountdown(timeRemaining), //And question countdown. Rename to dashboard countdown?
                (Question question) => Clients.Caller.ShowQuestion(question.QuestionText));
        }

        public async Task JoinGame(string code, string name)
        {
            this.state.JoinGame(code, name, this.Context.ConnectionId, (string dashboardId, string[] playerNames) => Clients.Client(dashboardId).UpdatePlayerList(playerNames));
        }
    }
}

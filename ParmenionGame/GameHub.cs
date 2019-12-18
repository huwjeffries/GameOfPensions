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
            this.state.CreateGame("DEF", (int timeRemaining) => Clients.Caller.SendAsync("JoinGameCountdown", timeRemaining));
        }

        public async Task JoinGame(string code, string name)
        {
            //this.state.JoinGame(code, this.Context.ConnectionId);
        }
    }
}

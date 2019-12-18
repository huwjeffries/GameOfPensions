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
            this.state.JoinGame("DEF", (int timeRemaining) => Clients.Caller.SendAsync("JoinGameCountdown", timeRemaining));
        }
    }
}

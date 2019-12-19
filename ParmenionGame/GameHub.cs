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
            await this.state.RegisterDashboard(this.Context.ConnectionId);
        }

        public async Task RegisterPlayer(string code, string name)
        {
            await this.state.RegisterPlayer(code, name, this.Context.ConnectionId);
        }
    }
}

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

        public bool IsGameInProgress()
        {
            return state.IsGameInProgress();
        }

        public async Task RegisterPlayer(string code, string name)
        {
            await this.state.RegisterPlayer(code, name, this.Context.ConnectionId);
        }

        public async Task PlayerAnswer(int answerIndex)
        {
            await state.PlayerAnswer(answerIndex, this.Context.ConnectionId);
        }
    }
}

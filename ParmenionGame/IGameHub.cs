using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public interface IGameHub
    {
        Task JoinGameCode(string code);
        Task JoinGameCountdown(int timeRemaining);
        Task ShowQuestion(string question);
        Task UpdatePlayerList(string[] playerNames);
    }
}

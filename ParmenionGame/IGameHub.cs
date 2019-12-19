using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public interface IGameHub
    {
        Task JoinGameCode(string code);
        Task ShowIncorrectGameCode();
        Task Countdown(int timeRemaining);
        Task ShowQuestionText(string question);
        Task ShowQuestionAnswers(string[] answers);
        Task UpdatePlayerList(string[] playerNames);
    }
}

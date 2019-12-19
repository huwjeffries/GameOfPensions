using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public interface IGameHub
    {
        Task ShowDashboardJoinGameCode(string code);
        Task ShowPlayerIncorrectGameCode();
        Task ShowPlayerNewGameStarted();
        Task Countdown(int timeRemaining);
        Task ShowDashboardQuestionText(string question);
        Task ShowPlayerQuestionAnswers(string[] answers);
        Task ShowDashboardPlayerList(string[] playerNames);
        Task Disconnect();
    }
}

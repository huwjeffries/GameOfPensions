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
        Task ShowPlayerAcceptedGameCode();
        Task ShowPlayerGameInProgress();
        Task ShowPlayerNewGameReady();
        Task Countdown(int timeRemaining);
        Task ShowDashboardQuestionText(string question);
        Task ShowPlayerQuestionAnswers(string[] answers);
        Task ShowPlayerAnswerAccepted(int answerIndex);
        Task ShowDashboardPlayerList(string[] playerNames);
        Task ShowGameFinished(string message);
        Task Disconnect();
        
    }
}

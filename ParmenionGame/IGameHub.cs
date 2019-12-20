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
        Task ShowDashboardQuestionText(string question, string prompt, int playerAge);
        Task ShowPlayerQuestionAnswers(string[] answers, int savings, int pensionPot, int property);
        Task ShowPlayerAnswerAccepted(int answerIndex);
        Task ShowDashboardPlayerList(string[] playerNames);
        Task ShowGameFinished(string message);
        Task Disconnect();
        
    }
}

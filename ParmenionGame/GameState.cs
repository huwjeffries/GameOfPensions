using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class GameState
    {
        private int questionNumber = 0;
        private Question[] questions =
        {
            new Question()
            {
                QuestionText = "What do you get if you multiply 6 by 7?",
                Answers = new string[]{ "42", "35", "56"}
            },
            new Question()
            {
                QuestionText = "What is the Queen's favourite animal?",
                Answers = new string[]{ "Corgi", "Monkey", "Spider", "Horse" }
            }
        };

        private readonly ILogger<GameState> logger;
        private readonly IHubContext<GameHub, IGameHub> hubContext;        
        private List<Player> gamePlayers = new List<Player>();
        private string dashboardConnectionId;
        private string gameCode;
        private Countdown countdown = null;
        private bool isGameInProgres = false;

        public GameState(ILogger<GameState> logger, IHubContext<GameHub, IGameHub> hubContext)
        {
            this.logger = logger;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// Grab the connection ID of the dashboard and reset the game state.
        /// </summary>
        public async Task RegisterDashboard(string dashboardConnectionId)
        {
            this.dashboardConnectionId = dashboardConnectionId; //TODO - do we need to disconnect existing dashboard?
            await StartNewGame();
        }

        public bool IsGameInProgress()
        {
            return isGameInProgres;
        }

        /// <summary>
        /// Register a new player
        /// </summary>
        public async Task RegisterPlayer(string code, string name, string playerConnectionId)
        {
            logger.LogDebug($"'{name}' joined game '{code}'");
            //Check the player isn't already registered.
            if (!gamePlayers.Any(p => p.ConnectionId == playerConnectionId))
            {
                if(code.ToLower() == gameCode)
                {
                    gamePlayers.Add(new Player(name, playerConnectionId)); //TOOD - sanitise the name input.
                    await hubContext.Clients.Client(dashboardConnectionId).ShowDashboardPlayerList(gamePlayers.Select(p => p.Name).ToArray());
                } else
                {
                    await hubContext.Clients.Client(playerConnectionId).ShowPlayerIncorrectGameCode();
                }
            }
        }


        /// <summary>
        /// Reset the game state.
        /// </summary>
        private async Task StartNewGame()
        {
            //Stop any active timers
            if (countdown!=null)
            {
                countdown.StopTimer();
                countdown = null;
            }
            gamePlayers.Clear();  //TODO - do we need to disconnect existing players?
            questionNumber = 0;            
            gameCode = "def";  //TODO - generate random letters lowercase
            isGameInProgres = false;
            logger.LogDebug($"Created game with code '{gameCode}'");

            //Show the game code and start the countdown
            await hubContext.Clients.Client(dashboardConnectionId).ShowDashboardJoinGameCode(gameCode);
            await hubContext.Clients.AllExcept(dashboardConnectionId).ShowPlayerNewGameReady();
            countdown = new Countdown(20, BroadcastCountdownProgress, BeginRounds);
        }

        private async Task BroadcastCountdownProgress(int timeRemaining)
        {
            await hubContext.Clients.All.Countdown(timeRemaining);
        }

        public async Task BeginRounds()
        {
            var allGameConnections = gamePlayers.Select(p => p.ConnectionId).ToList().Concat(new[] { dashboardConnectionId });
            await hubContext.Clients.AllExcept(allGameConnections.ToList()).ShowPlayerGameInProgress();
            await NextQuestion();
        }

        public async Task NextQuestion()
        {
            isGameInProgres = true; //Stop anyone else joining.

            if (questionNumber == questions.Length)
            {
                await EndGame();
            }
            else
            {
                //Send the question text to the dashboard and the answers to the mobile clients 
                await hubContext.Clients.Client(dashboardConnectionId).ShowDashboardQuestionText(questions[questionNumber].QuestionText);
                await hubContext.Clients.AllExcept(dashboardConnectionId).ShowPlayerQuestionAnswers(questions[questionNumber].Answers);
                questionNumber++;

                countdown = new Countdown(5, BroadcastCountdownProgress, NextQuestion);
            }
        }

        private async Task EndGame()
        {
            //TODO - Send Game Finished! Score table, etc.
            await hubContext.Clients.Client(dashboardConnectionId).ShowGameFinished("foo");
            foreach (var player in gamePlayers)
            {
                await hubContext.Clients.Client(player.ConnectionId).Disconnect();
            }

            countdown = new Countdown(10, BroadcastCountdownProgress, StartNewGame);
        }
    }
}

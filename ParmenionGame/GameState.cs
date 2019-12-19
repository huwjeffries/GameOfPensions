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

        /// <summary>
        /// Register a new player
        /// </summary>
        public async Task RegisterPlayer(string code, string name, string playerConnectionId) //, Action<string, string[]> updateDashboardAction)
        {
            logger.LogDebug($"'{name}' joined game '{code}'");
            //Check the player isn't already registered.
            if (!gamePlayers.Any(p => p.ConnectionId == playerConnectionId))
            {
                if(code == gameCode)
                {
                    gamePlayers.Add(new Player(name, playerConnectionId)); //TOOD - sanitise the name input.
                    await hubContext.Clients.Client(dashboardConnectionId).UpdatePlayerList(gamePlayers.Select(p => p.Name).ToArray());
                } else
                {
                    await hubContext.Clients.Client(playerConnectionId).ShowIncorrectGameCode();
                }
            }
        }


        /// <summary>
        /// Reset the game state.
        /// </summary>
        private async Task StartNewGame()
        {
            gamePlayers.Clear();  //TODO - do we need to disconnect existing players?
            questionNumber = 0;
            gameCode = "DEF";  //TODO - generate random letters
            logger.LogDebug($"Created game with code '{gameCode}'");
            
            //Show the game code and start the countdown
            await hubContext.Clients.Client(dashboardConnectionId).JoinGameCode(gameCode);
            new Countdown(10, BroadcastCountdownProgress, NextQuestion);
        }

        private async Task BroadcastCountdownProgress(int timeRemaining)
        {
            await hubContext.Clients.Client(dashboardConnectionId).Countdown(timeRemaining);
        }

        public async Task NextQuestion()
        {
            if(questionNumber == questions.Length)
            {
                //TODO - Send Game Finished! Score table, etc.
                int breakpoint = 1;
            } else
            {
                //Send the question text to the dashboard and the answers to the mobile clients 
                await hubContext.Clients.Client(dashboardConnectionId).ShowQuestionText(questions[questionNumber].QuestionText);
                await hubContext.Clients.AllExcept(dashboardConnectionId).ShowQuestionAnswers(questions[questionNumber].Answers);
                questionNumber++;
                
                new Countdown(10, BroadcastCountdownProgress, NextQuestion);
            }
        }

        
    }
}

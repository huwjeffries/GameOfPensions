using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class GameState
    {
        private int questionNumber = 0;

        private const int TimeBeforeGame = 17;
        private const int TimePerRound = 10;
        private const int TimeAfterGame = 600;

        private readonly ILogger<GameState> logger;
        private readonly IHubContext<GameHub, IGameHub> hubContext;
        private readonly QuestionsService questionsService;
        private List<Player> gamePlayers = new List<Player>();
        private string dashboardConnectionId;
        private string gameCode;
        private Countdown countdown = null;
        private bool isGameInProgres = false;

        public GameState(ILogger<GameState> logger, IHubContext<GameHub, IGameHub> hubContext, QuestionsService questionsService)
        {
            this.logger = logger;
            this.hubContext = hubContext;
            this.questionsService = questionsService;
        }

        /// <summary>
        /// Grab the connection ID of the dashboard and reset the game state.
        /// </summary>
        public async Task RegisterDashboard(string dashboardConnectionId)
        {
            this.dashboardConnectionId = dashboardConnectionId;
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
                    await hubContext.Clients.Client(playerConnectionId).ShowPlayerAcceptedGameCode();
                } else
                {
                    await hubContext.Clients.Client(playerConnectionId).ShowPlayerIncorrectGameCode();
                }
            }
        }

        public async Task PlayerAnswer(int answerIndex, string playerConnectionId)
        {            
            //TODO - We also have a possible race condition if the round has moved on while the message was in flight.
            await hubContext.Clients.Client(playerConnectionId).ShowPlayerAnswerAccepted(answerIndex);
            gamePlayers.Single(p => p.ConnectionId == playerConnectionId).SelectedAnswer = answerIndex;
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
            gameCode = GenerateRandomGameCode();
            isGameInProgres = false;
            logger.LogDebug($"Created game with code '{gameCode}'");

            //Show the game code and start the countdown
            await hubContext.Clients.Client(dashboardConnectionId).ShowDashboardJoinGameCode(gameCode);
            await hubContext.Clients.AllExcept(dashboardConnectionId).ShowPlayerNewGameReady();
            countdown = new Countdown(TimeBeforeGame, BroadcastCountdownProgress, BeginRounds);
        }

        private string GenerateRandomGameCode()
        {
            Random random = new Random();
            var sb = new StringBuilder();
            for (int x = 0; x < 3; x++)
            {
                int a = random.Next(0, 26);
                sb.Append((char)('a' + a));
            }
            return sb.ToString();
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
            if (isGameInProgres)
            {
                var currentQuestion = questionsService.GetQuestion(questionNumber);

                foreach (var player in gamePlayers)
                {
                    currentQuestion.Answers[player.SelectedAnswer].Effect(player);
                    player.SelectedAnswer = 0;
                }

                questionNumber++;
            }

            isGameInProgres = true; //Stop anyone else joining.

            var question = questionsService.GetQuestion(questionNumber);

            if (question == null)
            {
                foreach (var player in gamePlayers)
                {
                    var finalQuestion = questionsService.GetQuestion(questionNumber - 1);
                    ApplyYears(player, 65 - finalQuestion.Age);
                }
                await EndGame();
            }
            else
            {                
                //Send the question text to the dashboard and the answers to the mobile clients
                await hubContext.Clients.Client(dashboardConnectionId).ShowDashboardQuestionText(question.QuestionText, question.Prompt);

                var numberOfYears = questionNumber > 0 ? question.Age - questionsService.GetQuestion(questionNumber - 1).Age : 0;

                foreach (var player in gamePlayers)
                {
                    ApplyYears(player, numberOfYears);

                    await hubContext.Clients.Client(player.ConnectionId).ShowPlayerQuestionAnswers(
                        question.Answers.Select(a => a.Text).ToArray(),
                        (int)player.Savings,
                        (int)player.PensionPot,
                        (int)player.Property
                        );
                }

                countdown = new Countdown(TimePerRound, BroadcastCountdownProgress, NextQuestion);
            }
        }

        private async Task EndGame()
        {
            //TODO - Send Game Finished! Score table, etc.
            var playerResults = JsonConvert.SerializeObject(gamePlayers.OrderByDescending(p => p.NetWorth));
            await hubContext.Clients.Client(dashboardConnectionId).ShowGameFinished(playerResults);
            foreach (var player in gamePlayers)
            {
                // Sends a message requesting the players disconnect. We can't break the connection from server-side code.
                await hubContext.Clients.Client(player.ConnectionId).Disconnect();
            }

            countdown = new Countdown(TimeAfterGame, BroadcastCountdownProgress, StartNewGame);
        }

        private void ApplyYears(Player p, int numberOfYears)
        {
            if (numberOfYears <= 0)
            {
                return;
            }

            p.Income *= (decimal)Math.Pow(1.03, numberOfYears);
            p.PensionPot += p.Income * p.PensionContribution * numberOfYears;
            p.Property *= (decimal)Math.Pow(1.01, numberOfYears);
            p.Savings += p.Income * 0.03m * numberOfYears;
        }
    }
}

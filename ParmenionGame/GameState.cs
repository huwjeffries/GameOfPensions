using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        private Action<int> countdownProgressAction = null;
        private Action<Question> nextQuestionAction = null;

        private readonly ILogger<GameState> logger;
        private Timer _timer;
        private int countdownTime = 0;

        private Dictionary<string, List<Player>> GamePlayers;
        private Dictionary<string, string> Dashboards;

        public GameState(ILogger<GameState> logger)
        {
            this.logger = logger;
            this.GamePlayers = new Dictionary<string, List<Player>>();
            this.Dashboards = new Dictionary<string, string>();
        }

        public void JoinGame(string code, string name, string connectionId, Action<string, IEnumerable<string>> updateDashboardAction)
        {
            logger.LogDebug($"'{name}' joined game '{code}'");
            if (this.GamePlayers.ContainsKey(code))
            {
                this.GamePlayers[code].Add(new Player(name, connectionId));
                updateDashboardAction(this.Dashboards[code], this.GamePlayers[code].Select(p => p.Name));
            }
            else
            {
                // Handle error for unmatched code
            }
        }

        public void JoinGameCountdown(string code, string connectionId, Action<int> countdownProgressAction, Action<Question> nextQuestionAction)
        {
            questionNumber = 0;
            this.countdownProgressAction = countdownProgressAction;
            this.nextQuestionAction = nextQuestionAction;
            var countdown = new Countdown(10, countdownProgressAction, NextQuestion);

            if (!GamePlayers.ContainsKey(code))
            {
                this.GamePlayers.Add(code, new List<Player>());
                this.Dashboards.Add(code, connectionId);
                logger.LogDebug($"Created game with code '{code}'");
            }
        }

        public void NextQuestion()
        {
            if(questionNumber == questions.Length)
            {
                //Game finished#
                int breakpoint = 1;
            } else
            {
                nextQuestionAction(questions[questionNumber++]);
                var countdown = new Countdown(10, countdownProgressAction, NextQuestion);
            }
        }

        
    }
}

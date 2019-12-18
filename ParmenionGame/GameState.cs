using System;
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

        public GameState()
        {
        }

        public void JoinGameCountdown(string code, Action<int> countdownProgressAction, Action<Question> nextQuestionAction)
        {
            questionNumber = 0;
            this.countdownProgressAction = countdownProgressAction;
            this.nextQuestionAction = nextQuestionAction;
            var countdown = new Countdown(10, countdownProgressAction, NextQuestion);
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

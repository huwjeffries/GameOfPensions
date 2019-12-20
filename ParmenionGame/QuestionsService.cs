using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class QuestionsService
    {
        //Todo - these could come from a config file somewhere and be randomised per game.
        private Question[] questions =
{
            new Question()
            {
                QuestionText = "A contract job opportunity has come up. Do you leave your permanent job to take that?",
                Answers = new []{
                    new Answer("No", p => p.Income = 30000),
                    new Answer("Yes", p => { p.Income = 50000; p.SelfEmployed = true; })
                },
                Age = 25
            },
            new Question()
            {
                QuestionText = "Do you want to add a portion of your income to a pension scheme?",
                Prompt = "Parmenion employees get a 16% + 2% matched pension contribution on top of their salary",
                Answers = new []{
                    new Answer("None", p => { p.PensionContribution = p.SelfEmployed ? 0m : 0.02m; }),
                    new Answer("2%", p => { p.Income = p.Income * 0.98m; p.PensionContribution = p.SelfEmployed ? 0.02m : 0.04m; }),
                    new Answer("10%", p => { p.Income = p.Income * 0.9m; p.PensionContribution = p.SelfEmployed ? 0.10m : 0.15m; }),
                    new Answer("20%", p => { p.Income = p.Income * 0.8m; p.PensionContribution = p.SelfEmployed ? 0.20m : 0.25m; }),
                },
                Age = 27
            },
            new Question()
            {
                QuestionText = "Do you want to take out private medical insurance and income protection insurance?",
                Prompt = "Parmenion employees get access to both of these at a very large discount",
                Answers = new[] {
                    new Answer("No", p => { }),
                    new Answer("Yes", p => { p.Income -= 600; p.Insurance = true; })
                },
                Age = 30
            },            
            new Question()
            {
                QuestionText = "You get married! (Congratulations!). Do you want to have children?",
                Prompt = "Parmenion offers some fantastic benefits to parents.",
                Answers = new[] {
                    new Answer("None", p => p.Income *= 1.5m),
                    new Answer("One", p => { p.Income *= 1.3m; p.Children = 1; }),
                    new Answer("Two", p => { p.Income *= 1.1m; p.Children = 2; } ),
                    new Answer("Three", p => { p.Income *= 0.9m; p.Children = 3; }),
                },
                Age = 33
            },
            new Question()
            {
                QuestionText = "Do you want to buy a house?",
                Answers = new[] {
                    new Answer("No", p => { }),
                    new Answer("Yes", p => {
                        p.Income -= p.Children > 1 ? 4000m : 3000m;
                        p.Property = p.Children > 1 ? 350000 : 250000;
                        p.Savings -= p.Children > 1 ? 40000m : 30000m; })
                },
                Age = 35
            },
            new Question()
            {
                QuestionText = "You receive an inhertance of £100K. What do you want to do with it?",
                Prompt = "Parmenion offers preferential rates to staff investing in Parmenion ISAs / Pensions",
                Answers = new []{
                    new Answer("Keep it in the bank", p => p.Savings += 100000m),
                    new Answer("Invest in property", p => { p.Property += 100000; p.Income += 4000m; }),
                    new Answer("Stocks and shares", p => p.PensionPot += 100000m),
                    new Answer("Go on an amazing holiday", p => { })
                },
                Age = 45
            },
            new Question()
            {
                QuestionText = "You have suffered a serious injury.",
                Prompt = "Parmenion's Private Medical Cover covers a wide range of illnesses and conditions",
                Answers = new []{
                    new Answer("Ouch!", p => p.Savings -= p.Insurance ? 0m : p.Income / 2)
                },
                Age = 50
            },
            new Question()
            {
                QuestionText = "A friend tells you about a new CryptoCurrency. Do you want to buy a significant amount?",
                Answers = new [] {
                    new Answer("No", p => { }),
                    new Answer("Buy £5000", p => { p.Savings += new Random().Next() % 3 == 0 ? 15000m : -5000m; p.Crypto = true; }),
                    new Answer("Buy £50000", p => { p.Savings += new Random().Next() % 3 == 0 ? 150000m : -50000m; p.Crypto = true; }),
                },
                Age = 60
            }
        };

        public Question GetQuestion(int questionIndex)
        {
            if (questionIndex < questions.Length)
            {
                return questions[questionIndex];
            }
            return null;
        }
    }
}

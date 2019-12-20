using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParmenionGame
{
    public class Question
    {
        public string QuestionText { get; set; }
        public string Prompt { get; set; }
        public Answer[] Answers { get; set; }  
        public int Age { get; set; }
    }
}

using System;

namespace ParmenionGame
{
    public class Answer
    {
        public string Text { get; }
        public Action<Player> Effect { get; }

        public Answer(string text, Action<Player> effect)
        {
            this.Text = text;
            this.Effect = effect;
        }
    }
}

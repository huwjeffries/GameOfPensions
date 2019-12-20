namespace ParmenionGame
{
    public class Player
    {
        public Player(string name, string connectionId)
        {
            Name = name;
            ConnectionId = connectionId;
        }

        public string Name { get; }
        public string ConnectionId { get; }
        public int SelectedAnswer { get; set; }

        public bool SelfEmployed { get; set; }
        public decimal Income { get; set; }
        public decimal Savings { get; set; }
        public decimal PensionPot { get; set; } //7%
        public decimal PensionContribution { get; set; }
        public decimal Property { get; set; } //1%
        public bool Insurance { get; set; }
        public bool Crypto { get; set; }
        public int Children { get; set; }

        public decimal NetWorth { 
            get
            {
                return Savings + Property + PensionPot;
            } 
        }

    }
}

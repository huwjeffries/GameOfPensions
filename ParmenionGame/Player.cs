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
    }
}

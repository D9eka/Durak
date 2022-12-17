namespace Durak
{
    public class Player
    {
        public readonly string NickName;
        public List<Card> Hand = new();

        public Player(string nickName)
        {
            NickName = nickName;
        }

        public bool HaveCards()
        {
            return Hand.Count > 0;
        }
    }
}
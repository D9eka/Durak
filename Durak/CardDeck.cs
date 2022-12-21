using System.Text;

namespace Durak
{
    public class CardDeck
    {
        public readonly List<Card> Deck;
        public readonly Mark Trumb;
        private Random Random = new();

        public CardDeck()
        {
            Deck = new();
            CreateDeck();
            Shuffle();
            Trumb = ChooseTrump();
        }

        public void CreateDeck()
        {
            for (int i = 0; i < Enum.GetNames(typeof(Power)).Length; i++)
                for (int j = 0; j < Enum.GetNames(typeof(Mark)).Length; j++)
                    Deck.Add(new Card((Power)i, (Mark)j));
        }

        public void Shuffle()
        {
            for (int i = Deck.Count - 1; i > 0; i--)
            {
                int j = Random.Next(i + 1);
                (Deck[i], Deck[j]) = (Deck[j], Deck[i]);
            }
        }

        public Mark ChooseTrump()
        {
            return (Mark)Random.Next(Enum.GetNames(typeof(Mark)).Length - 1);
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append("Колода:\n");
            foreach (var card in Deck)
                str.Append(card.ToString() + '\n');
            return str.ToString();
        }
    }
}

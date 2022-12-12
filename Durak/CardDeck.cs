using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public class CardDeck
    {
        public List<Card> Deck = new();
        public Mark Trumb;
        public Random random = new();

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
                int j = random.Next(i + 1);
                (Deck[i], Deck[j]) = (Deck[j], Deck[i]);
            }
        }

        public void ChooseTrump()
        {
            Trumb = (Mark)random.Next(Enum.GetNames(typeof(Mark)).Length - 1);
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

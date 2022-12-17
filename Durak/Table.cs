using System.Text;

namespace Durak
{
    public class Table
    {
        public CardDeck Deck = new();
        public Card[] AttackCards = new Card[Rules.CardsOnHand];
        public Card[] DefendCards = new Card[Rules.CardsOnHand];
        public List<Card> DiscardPile = new();

        public void RemoveCards(List<Card> destination)
        {
            for (int i = 0; i < AttackCards.Length; i++)
            {
                if (AttackCards[i] != null)
                {
                    destination.Add(AttackCards[i]);
                    AttackCards[i] = null;
                }
                if (DefendCards[i] != null)
                {
                    destination.Add(DefendCards[i]);
                    DefendCards[i] = null;
                }
            }
        }

        public void PutCard(Card card)
        {
            for (int i = 0; i < this.AttackCards.Length; i++)
                if (this.AttackCards[i] == null)
                {
                    this.AttackCards[i] = card;
                    break;
                }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < this.AttackCards.Length; i++)
            {
                if (this.AttackCards[i] != null)
                {
                    result.Append(string.Format("({0}) {1} ", i + 1, this.AttackCards[i]));
                    if (this.DefendCards[i] != null)
                        result.Append(this.DefendCards[i] + "\n");
                    else
                        result.Append("|                  |\n");
                }
            }
            return result.ToString();
        }
    }
}

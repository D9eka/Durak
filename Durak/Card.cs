namespace Durak
{
    public class Card : IComparable<Card>
    {
        public static int Width = 20;
        public Power Power;
        public Mark Mark;

        public Card(Power power, Mark mark)
        {
            Power = power;
            Mark = mark;
        }

        public int CompareTo(Card other)
        {
            var result = this.Mark - other.Mark;
            if (result == 0)
                return this.Power - other.Power;
            return result;
        }

        public override string ToString()
        {
            return Screen.CreateJustifyString(Power.ToString() + ' ' + Mark.ToString(), Width);
        }
    }

    public enum Power
    {
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Quenn,
        King,
        Ace
    }

    public enum Mark
    {
        Hearts,
        Tiles,
        Clovers,
        Pikes
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public class Card : IComparable<Card>
    {
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
            var freeSpace = 15;
            freeSpace -= Power.ToString().Length;
            freeSpace -= Mark.ToString().Length;

            int leftSpace;
            int rightSpace;

            if (freeSpace % 2 == 0)
            {
                leftSpace = freeSpace / 2;
                rightSpace = freeSpace / 2;
            }
            else
            {
                leftSpace = freeSpace / 2;
                rightSpace = (freeSpace / 2) + 1;
            }

            var result = new StringBuilder("|");
            for (int i = 0; i < leftSpace; i++)
                result.Append(" ");
            result.Append(Power + " " + Mark);
            for (int i = 0; i < rightSpace; i++)
                result.Append(" ");
            result.Append("|");
            return result.ToString();
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

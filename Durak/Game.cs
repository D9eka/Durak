using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public class Game
    {
        public int PlayersCount;
        public List<Player> Players = new();
        public Table Table = new();
        private int CurrentMove = 0;

        public Game(int playersCount)
        {
            if (playersCount < 0 || playersCount > 6)
                throw new Exception("Невозможно создать игру с таким количеством игроков");
            PlayersCount = playersCount;
            Console.WriteLine("Введите ники игроков:");
            for (int i = 0; i < PlayersCount; i++)
                Players.Add(new Player(Console.ReadLine(), i));
        }

        private void GiveCardToPlayer(Player player, int count)
        {
            if (count > Table.Deck.Deck.Count)
                count = Table.Deck.Deck.Count;

            for (int i = 0; i < count; i++)
            {
                player.Hand.Add(Table.Deck.Deck[0]);
                Table.Deck.Deck.Remove(Table.Deck.Deck[0]);
            }

            player.Hand.Sort();
        }

        private int PlayersHaveCards()
        {
            var playersHaveCards = 0;
            foreach (var player in Players)
            {
                if (player.Hand.Count > 0)
                    playersHaveCards++;
            }

            return playersHaveCards;
        }

        public string CardToString(List<Card> list)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
                result.Append(String.Format("({0}) {1}\n", i + 1, list[i].ToString()));

            return result.ToString();
        }

        public string CardToString(Card[] array)
        {
            var result = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                    result.Append(String.Format("({0}) {1}\n", i + 1, array[i].ToString()));
                else
                    result.Append(String.Format("({0}) |                |\n", i + 1));
            }

            return result.ToString();
        }

        public void StartGame()
        {
            Table.Deck.CreateDeck();
            Table.Deck.Shuffle();
            Table.Deck.ChooseTrump();
            Console.WriteLine("Козырь - {0}", Table.Deck.Trumb);

            foreach (var player in Players)
                GiveCardToPlayer(player, Rules.CardsOnHand);

            while (PlayersHaveCards() != 1)
            {
                var attackPlayer = Players[CurrentMove % PlayersCount];
                var defendPlayer = Players[(CurrentMove + 1) % PlayersCount];
                PlayerAttack(attackPlayer, defendPlayer);
                GiveCardToPlayer(attackPlayer, Rules.CardsOnHand - attackPlayer.Hand.Count);
                GiveCardToPlayer(defendPlayer, Rules.CardsOnHand - defendPlayer.Hand.Count);
                Table.RemoveCards(Table.DiscardPile);
                CurrentMove++;
            }

            foreach (var player in Players)
                if (player.Hand.Count > 0)
                {
                    Console.WriteLine("{0} - дурак!", player.NickName);
                    return;
                }
        }

        private void PlayerAttack(Player attackPlayer, Player defendPlayer)
        {
            Console.WriteLine("{0},\n" +
                              "Козырь - {1}\n" +
                              "Ваша рука:\n" +
                              "{2}" +
                              "Карты на столе:\n" +
                              "    |     Атака      | |     Защита     |\n" +
                              "{3}\nВведите число карты, которой вы хотите атаковать" +
                              "\nВведите\"Бито\", чтобы завершить ход",
                              attackPlayer.NickName, Table.Deck.Trumb, CardToString(attackPlayer.Hand), TableToString());

            var input = Console.ReadLine();
            if (input == "Бито")
            {
                Console.WriteLine("Завершение хода");
                return;
            }
            if (!int.TryParse(input, out int num))
            {
                Console.WriteLine("Не удалось найти карту. Проверьте правильность введённых чисел");
                PlayerAttack(attackPlayer, defendPlayer);
                return;
            }

            Card card = attackPlayer.Hand[int.Parse(input) - 1];
            if (Table.AttackCards[0] == null)
            {
                PutCard(card);
                attackPlayer.Hand.Remove(card);
                PlayerDefend(defendPlayer, attackPlayer);
            }
            else
            {
                for (int i = 0; i < Table.AttackCards.Length; i++)
                    if ((Table.AttackCards[i] != null && card.Power == Table.AttackCards[i].Power) ||
                        (Table.DefendCards[i] != null && card.Power == Table.DefendCards[i].Power))
                    {
                        PutCard(card);
                        attackPlayer.Hand.Remove(card);
                        PlayerDefend(defendPlayer, attackPlayer);
                        return;
                    }
                Console.WriteLine("Вы не можете положить эту карту");
                Console.WriteLine();
                PlayerAttack(attackPlayer, defendPlayer);
                return;
            }
        }

        private void PutCard(Card card)
        {
            for (int i = 0; i < Table.AttackCards.Length; i++)
                if (Table.AttackCards[i] == null)
                {
                    Table.AttackCards[i] = card;
                    break;
                }
        }

        private string TableToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < Table.AttackCards.Length; i++)
            {
                if (Table.AttackCards[i] != null)
                {
                    result.Append(string.Format("({0}) {1} ", i + 1, Table.AttackCards[i]));
                    if (Table.DefendCards[i] != null)
                        result.Append(Table.DefendCards[i] + "\n");
                    else
                        result.Append("|                |\n");
                }
            }
            return result.ToString();
        }

        private void PlayerDefend(Player defendPlayer, Player attackPlayer)
        {
            Console.WriteLine("{0},\n" +
                              "Козырь - {1}\n" +
                              "Ваша рука:\n" +
                              "{2}\n" +
                              "Карты на столе:\n" +
                              "    |     Атака      | |     Защита     |\n" +
                              "{3}" +
                              "Введите число карты, которую вы хотите покрыть, " +
                              "и число карты, которой вы хотите покрыть\n" +
                              "Введите \"Взять\", чтобы завершить ход",
                              defendPlayer.NickName, Table.Deck.Trumb, CardToString(defendPlayer.Hand), TableToString());

            var input = Console.ReadLine().Split();
            if (input[0] == "Взять")
            {
                Table.RemoveCards(defendPlayer.Hand);
                defendPlayer.Hand.Sort();
                CurrentMove++;
                return;
            }
            if (!int.TryParse(input[0], out int num))
            {
                Console.WriteLine("Не удалось найти карту. Проверьте правильность введённых чисел");
                PlayerDefend(defendPlayer, attackPlayer);
                return;
            }

            int[] ints = new int[2] { int.Parse(input[0]) - 1, int.Parse(input[1]) - 1 };

            Card attackCard = Table.AttackCards[ints[0]];
            Card defendCard = defendPlayer.Hand[ints[1]];
            if ((defendCard.Mark == attackCard.Mark && defendCard.Power > attackCard.Power) ||
               (defendCard.Mark == Table.Deck.Trumb && attackCard.Mark != Table.Deck.Trumb))
            {
                Table.DefendCards[ints[0]] = defendPlayer.Hand[ints[1]];
                defendPlayer.Hand.Remove(defendPlayer.Hand[ints[1]]);
            }
            else
            {
                Console.WriteLine("Вы не можете покрыть этой карто. Выберите другую");
                PlayerDefend(defendPlayer, attackPlayer);
            }

            if (CardsCount(Table.AttackCards) == 6 || defendPlayer.Hand.Count == 0)
            {
                Console.WriteLine("Бито");
                return;
            }
            PlayerAttack(attackPlayer, defendPlayer);
        }

        private int CardsCount(Card[] array)
        {
            int count = 0;
            foreach (var card in array)
                if (card != null)
                    count++;
            return count;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public static class Rules
    {
        public static int CardsOnHand = 6;
        public static int MaxPlayers = 6;

        public static void GiveCardToPlayer(Player player, Table table)
        {
            var count = CardsOnHand - player.Hand.Count;
            if (count > table.Deck.Deck.Count)
                count = table.Deck.Deck.Count;

            for (int i = 0; i < count; i++)
            {
                player.Hand.Add(table.Deck.Deck[0]);
                table.Deck.Deck.Remove(table.Deck.Deck[0]);
            }

            player.Hand.Sort();
        }

        public static void CheckAttackMove(Game game, Player player, string input, Table table)
        {
            if (input.ToLower() == "бито")
            {
                if (table.AttackCards[0] == null)
                {
                    Console.WriteLine("Нужно атаковать хотя бы одной картой, чтобы закончить ход");
                    game.PlayerAttack(player);
                    return;
                }
                Console.WriteLine("Завершение хода...");
                game.EndMove = true;
                return;
            }

            if (!CheckInput(input, player.Hand.Count))
            {
                game.PlayerAttack(player);
                return;
            }

            Card card = player.Hand[int.Parse(input) - 1];

            if (table.AttackCards[0] == null)
            {
                table.PutCard(card);
                player.Hand.Remove(card);
            }
            else
            {
                for (int i = 0; i < table.AttackCards.Length; i++)
                    if ((table.AttackCards[i] != null && card.Power == table.AttackCards[i].Power) ||
                        (table.DefendCards[i] != null && card.Power == table.DefendCards[i].Power))
                    {
                        table.PutCard(card);
                        player.Hand.Remove(card);
                        return;
                    }
                Console.WriteLine("Вы не можете положить эту карту");
                Console.WriteLine();
                game.PlayerAttack(player);
                return;
            }
        }

        public static void CheckDefendMove(Game game, Player attackPlayer, Player defendPlayer, string input, Table table)
        {
            if (input.ToLower() == "взять")
            {
                var throwsCardsMove = game.PlayerThrowsCards(attackPlayer, defendPlayer);
                CheckThrowsCardsMove(game, attackPlayer, defendPlayer, throwsCardsMove, table);
                table.RemoveCards(defendPlayer.Hand);
                defendPlayer.Hand.Sort();
                game.CurrentMove++;
                game.EndMove = true;
                return;
            }

            if (!CheckInput(input, defendPlayer.Hand.Count))
            {
                game.PlayerDefend(defendPlayer);
                return;
            }

            int defendCardNum = int.Parse(input) - 1;
            int attackCardNum = CardsOnHand - 1;

            for (int i = 0; i < table.AttackCards.Length - 1; i++)
                if (table.AttackCards[i] == null)
                {
                    attackCardNum = i - 1;
                    break;
                }

            Card attackCard = table.AttackCards[attackCardNum];
            Card defendCard = defendPlayer.Hand[defendCardNum];

            if ((defendCard.Mark == attackCard.Mark && defendCard.Power > attackCard.Power) ||
               (defendCard.Mark == table.Deck.Trumb && attackCard.Mark != table.Deck.Trumb))
            {
                table.DefendCards[attackCardNum] = defendPlayer.Hand[defendCardNum];
                defendPlayer.Hand.Remove(defendPlayer.Hand[defendCardNum]);
            }
            else
            {
                Console.WriteLine("Вы не можете покрыть этой картой. Выберите другую");
                game.PlayerDefend(defendPlayer);
            }

            if (CardsCount(table.AttackCards) == 6 || defendPlayer.Hand.Count == 0)
            {
                Console.WriteLine("Бито");
                game.EndMove = true;
                return;
            }
        }

        public static void CheckThrowsCardsMove(Game game, Player attackPlayer, Player defendPlayer, string input, Table table)
        {
            if (input.ToLower() == "бито")
            {
                Console.WriteLine("Завершение хода...");
                game.EndMove = true;
                return;
            }

            var cardsNumber = input.Split();
            for(int i = cardsNumber.Length - 1; i >= 0; i--)
            {
                if (!CheckInput(cardsNumber[i], attackPlayer.Hand.Count))
                {
                    game.PlayerThrowsCards(attackPlayer, defendPlayer);
                    return;
                }
                CheckAttackMove(game, attackPlayer, cardsNumber[i], table);
            }               
        }

        private static bool CheckInput(string input, int cardsCount)
        {
            if (!int.TryParse(input, out int num))
            {
                Console.WriteLine("Не удалось найти карту. Проверьте правильность введённых чисел");
                return false;
            }

            if (int.Parse(input) - 1 > cardsCount)
            {
                Console.WriteLine("У вас нет столько карт. Введите другое число");
                return false;
            }

            return true;
        }

        private static int CardsCount(Card[] array)
        {
            int count = 0;
            foreach (var card in array)
                if (card != null)
                    count++;
            return count;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Durak
{
    public class Game
    {
        public int PlayersCount;
        public List<Player> Players = new();
        public Table Table = new();
        public int CurrentMove = 0;
        public bool EndMove = false;

        public Game(int playersCount)
        {
            if (playersCount < 0 || playersCount > 6)
                throw new Exception("Невозможно создать игру с таким количеством игроков");
            PlayersCount = playersCount;
            Console.WriteLine("Введите ники игроков:");
            for (int i = 0; i < PlayersCount; i++)
                Players.Add(new Player(Console.ReadLine(), i));
        }

        public void StartGame()
        {
            Table.Deck.CreateDeck();
            Table.Deck.Shuffle();
            Table.Deck.ChooseTrump();

            foreach (var player in Players)
                Rules.GiveCardToPlayer(player, Table);

            while (PlayersHaveCards() != 1)
            {
                var attackPlayer = Players[CurrentMove % PlayersCount];
                var defendPlayer = Players[(CurrentMove + 1) % PlayersCount];

                while (EndMove != true)
                {
                    var attackMove = PlayerAttack(attackPlayer);
                    Rules.CheckAttackMove(this, attackPlayer, attackMove, Table);

                    if (EndMove)
                        break;

                    var defendMove = PlayerDefend(defendPlayer);
                    Rules.CheckDefendMove(this, defendPlayer, defendMove, Table);
                }
                
                //Rules.GiveCardToPlayer(attackPlayer, Table);
                //Rules.GiveCardToPlayer(defendPlayer, Table);
                Table.RemoveCards(Table.DiscardPile);
                CurrentMove++;
                EndMove = false;
            }

            foreach (var player in Players)
                if (player.Hand.Count > 0)
                {
                    Console.WriteLine("{0} - дурак!", player.NickName);
                    return;
                }
        }

        public string PlayerAttack(Player attackPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("{0}, Вы атакуете\n" +
                              "Козырь - {1}\n" +
                              "Ваша рука:\n" +
                              "{2}" +
                              "Карты на столе:\n" +
                              "    |      Атака       | |      Защита      |\n" +
                              "{3}\nВведите число карты, которой вы хотите атаковать" +
                              "\nВведите \"Бито\", чтобы завершить ход",
                              attackPlayer.NickName, Table.Deck.Trumb, CardsToString(attackPlayer.Hand), Table.ToString());
            return Console.ReadLine();
        }

        public string PlayerDefend(Player defendPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("{0}, Вы защищаетесь\n" +
                              "Козырь - {1}\n" +
                              "Ваша рука:\n" +
                              "{2}\n" +
                              "Карты на столе:\n" +
                              "    |      Атака       | |      Защита      |\n" +
                              "{3}" +
                              "Введите число карты, которой вы хотите покрыть\n" +
                              "Введите \"Взять\", чтобы завершить ход",
                              defendPlayer.NickName, Table.Deck.Trumb, CardsToString(defendPlayer.Hand), Table.ToString());
            return Console.ReadLine();
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

        public string CardsToString(List<Card> list)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
                result.Append(String.Format("({0}) {1}\n", i + 1, list[i].ToString()));

            return result.ToString();
        }
    }
}

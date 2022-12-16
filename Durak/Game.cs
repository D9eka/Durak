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
        public List<Player> Players = new();
        public Table Table = new();
        public int CurrentMove;
        public bool EndMove = false;

        public Game(List<Player> players)
        {
            Players = players;
        }

        public void StartGame()
        {
            Table.Deck.CreateDeck();
            Table.Deck.Shuffle();
            Table.Deck.ChooseTrump();

            foreach (var player in Players)
                Rules.GiveCardToPlayer(player, Table);

            CurrentMove = new Random().Next(Players.Count);
            var playersWin = 0;
            
            while (Players.Count() != 1)
            {
                var attackPlayer = Players[CurrentMove % Players.Count];
                var defendPlayer = Players[(CurrentMove + 1) % Players.Count];

                while (EndMove != true)
                {
                    var attackMove = PlayerAttack(attackPlayer);
                    Rules.CheckAttackMove(this, attackPlayer, attackMove, Table);

                    if (EndMove)
                        break;

                    var defendMove = PlayerDefend(defendPlayer);
                    Rules.CheckDefendMove(this, attackPlayer, defendPlayer, defendMove, Table);
                }
                
                Rules.GiveCardToPlayer(attackPlayer, Table);
                Rules.GiveCardToPlayer(defendPlayer, Table);
                Table.RemoveCards(Table.DiscardPile);
                CurrentMove++;
                EndMove = false;
                CheckWinnedPlayers(ref playersWin);
            }

            foreach (var player in Players)
                if (player.Hand.Count > 0)
                {
                    Console.WriteLine("{0} - дурак!", player.NickName);
                    Console.WriteLine("Хотите сыграть ещё раз? (да/нет)");
                    var input = Console.ReadLine();
                    if (input.ToLower() == "да")
                        Program.Main();
                    return;
                }
        }

        private void CheckWinnedPlayers(ref int playersWin)
        {
            for (int i = Players.Count - 1; i >= 0; i--)
                if (!Players[i].HaveCards())
                {
                    playersWin++;
                    Console.WriteLine("Игрок {0} занимает {1} место!", Players[i].NickName, playersWin);
                    Players.Remove(Players[i]);
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
                              attackPlayer.NickName, Table.Deck.Trumb, Card.CardsToString(attackPlayer.Hand), Table.ToString());
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
                              defendPlayer.NickName, Table.Deck.Trumb, Card.CardsToString(defendPlayer.Hand), Table.ToString());
            return Console.ReadLine();
        }

        public string PlayerThrowsCards(Player attackPlayer, Player defendPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("{0}, {1} берёт карты\n" +
                              "Козырь - {2}\n" +
                              "Ваша рука:\n" +
                              "{3}\n" +
                              "Карты на столе:\n" +
                              "    |      Атака       | |      Защита      |\n" +
                              "{4}" +
                              "Введите числа карт, которые вы хотите подкинуть\n" +
                              "Введите \"Бито\", чтобы завершить ход",
                              attackPlayer.NickName, defendPlayer.NickName, Table.Deck.Trumb, Card.CardsToString(attackPlayer.Hand), Table.ToString());
            return Console.ReadLine();
        }
    }
}

namespace Durak
{
    public class Game
    {
        public readonly List<Player> Players;
        public readonly Table Table;
        public int CurrentMove;
        public bool EndMove = false;

        public Game(List<Player> players)
        {
            Players = players;
            Table = new();
        }

        public void StartGame()
        {
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
                    Thread.Sleep(5000);
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

        public int PlayerAttack(Player attackPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Screen playerScreen = new PlayerScreen(attackPlayer, new string[] { "Вы атакуете!", "Бито" });
            Screen tableScreen = new TableScreen(Table);
            var selectedIndex = Screen.Run(attackPlayer.Hand.Count + 1, playerScreen, tableScreen);
            return selectedIndex;
        }

        public int PlayerDefend(Player defendPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Screen playerScreen = new PlayerScreen(defendPlayer, new string[] { "Вы защищаетесь!", "Взять" });
            Screen tableScreen = new TableScreen(Table);
            var selectedIndex = Screen.Run(defendPlayer.Hand.Count + 1, playerScreen, tableScreen);
            return selectedIndex;
        }

        public int PlayerThrowsCards(Player attackPlayer, Player defendPlayer)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Screen playerScreen = new PlayerScreen(attackPlayer, new string[] { defendPlayer.NickName + " берет!", "Бито" });
            Screen tableScreen = new TableScreen(Table);
            var selectedIndex = Screen.Run(attackPlayer.Hand.Count + 1, playerScreen, tableScreen);
            return selectedIndex;
        }
    }
}
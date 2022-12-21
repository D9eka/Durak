namespace Durak
{
    public class Program
    {
        public static void Main()
        {
            Console.WindowHeight = Console.LargestWindowHeight;
            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.Title = "Durak";

            Console.Clear();
            GC.Collect();

            var mainMenuMenu = new List<string> { "Играть", "Выйти" };
            Screen mainMenu = new MenuScreen("Дурак", mainMenuMenu);
            var selectedIndex = Screen.Run(mainMenuMenu.Count, mainMenu);

            switch (selectedIndex)
            {
                case 1:
                    return;
                default:
                    break;
            }

            var choosePlayerCountMenuMenu = new List<string> { "2", "3", "4", "5", "6" };
            Screen choosePlayerCountMenu = new MenuScreen("Выберите количество игроков", choosePlayerCountMenuMenu);
            selectedIndex = Screen.Run(choosePlayerCountMenuMenu.Count, choosePlayerCountMenu);

            var playersCount = selectedIndex + 2;

            Console.WriteLine("Введите ники игроков (в одну строку)");
            var input = Console.ReadLine().Split();

            while (!IsNickNamesCorrect(input, playersCount))
            {
                Console.WriteLine("Количество ников отличается от количества игроков. Необходимо ввести {0} ника (каждый ник отделён пробелом)", playersCount);
                input = Console.ReadLine().Split();
            }

            var players = new List<Player>();
            for (int i = 0; i < playersCount; i++)
                players.Add(new Player(input[i]));

            var game = new Game(players);
            game.StartGame();

            var restartMenuMenu = new List<string> { "Да", "Нет" };
            Screen restartMenu = new MenuScreen("Хотите сыграть ещё раз?", mainMenuMenu);
            selectedIndex = Screen.Run(restartMenuMenu.Count, restartMenu);

            if (selectedIndex == 0)
                Program.Main();
        }

        private static bool IsNickNamesCorrect(string[] input, int playersCount)
        {
            if (input.Length < playersCount)
                return false;

            return true;
        }
    }
}
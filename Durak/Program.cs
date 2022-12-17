using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Durak
{
    public class Program
    {
        public static void Main()
        {
            Console.WindowHeight = Console.LargestWindowHeight;
            Console.WindowWidth = 80;
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

            Console.WriteLine("Введите количество игроков");
            var playersCount = int.Parse(Console.ReadLine());
            if (playersCount < 0 || playersCount > Rules.MaxPlayers)
                throw new Exception("Невозможно создать игру с таким количеством игроков");

            var players = new List<Player>();

            Console.WriteLine("Введите ники игроков (в одну строку, максимум 10 символов)");
            var input = Console.ReadLine().Split();

            if (input.Length < playersCount)
                throw new Exception("Количество ником отличается от количества игроков");

            foreach (var nickName in input)
                if (nickName.Length > 10)
                    throw new Exception("Один из ников слишком длинный");

            for (int i = 0; i < playersCount; i++)
                players.Add(new Player(input[i]));

            var game = new Game(players);
            game.StartGame();

            var restartMenuMenu = new List<string> { "Да", "Нет" };
            Screen restartMenu = new MenuScreen("Хотите сыграть ещё раз?", mainMenuMenu);
            selectedIndex = Screen.Run(restartMenuMenu.Count, restartMenu);

            if (selectedIndex == 0)
            {
                Program.Main();
            }
        }
    }
}
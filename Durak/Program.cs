using System.Numerics;
using System.Text;

namespace Durak
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "Durak";

            Console.Clear();
            GC.Collect();

            Console.WriteLine("Введите количество игроков");
            var playersCount = int.Parse(Console.ReadLine());
            if (playersCount < 0 || playersCount > Rules.MaxPlayers)
                throw new Exception("Невозможно создать игру с таким количеством игроков");

            var players = new List<Player>();

            Console.WriteLine("Введите ники игроков (в одну строку)");
            var input = Console.ReadLine().Split();

            if(input.Length < playersCount)
                throw new Exception("Количество ником отличается от количества игроков");

            for (int i = 0; i < playersCount; i++)
                players.Add(new Player(input[i]));

            var game = new Game(players);
            game.StartGame();
        }
    }
}
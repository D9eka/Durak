using System.Numerics;
using System.Text;

namespace Durak
{
    

    

    

    

    

    

    class Program
    {
        static void Main()
        {
            Console.SetWindowSize(50, 25);
            Console.WriteLine("Введите количество игроков");
            var number = int.Parse(Console.ReadLine());
            var game = new Game(number);
            game.StartGame();
        }
    }
}
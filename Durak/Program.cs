using System.Numerics;
using System.Text;

namespace Durak
{
    class Program
    {
        static void Main()
        {
            var consoleWidth = 127;
            var consoleHeight = 30;
            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.Title = "Durak";
            /*var screenTop = '+' + new string('—', consoleWidth - 2) + "+\n";
            var screenLine = '|' + new string(' ', consoleWidth - 2) + "|\n";
            var screen = new StringBuilder(screenTop);
            for (int i = 0; i < consoleHeight - 2; i++)
                screen.Append(screenLine);
            screen.Append(screenTop);
            Console.WriteLine(screen.ToString());*/
            Console.WriteLine("Введите количество игроков");
            var number = int.Parse(Console.ReadLine());
            var game = new Game(number);
            game.StartGame();
        }
    }
}
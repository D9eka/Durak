using System.Text;

namespace Durak
{
    public class Screen
    {
        public static char AngleBorder = '+';
        public static char VerticalBorder = '|';
        public static char HorizontalBorder = '-';
        public static char SelectedIndexChar = '>';
        public static int SelectedIndex;
        static ConsoleKey[] UpKeys = new ConsoleKey[] { ConsoleKey.W, ConsoleKey.UpArrow };
        static ConsoleKey[] DownKeys = new ConsoleKey[] { ConsoleKey.S, ConsoleKey.DownArrow };
        static ConsoleKey[] ChoiceKeys = new ConsoleKey[] { ConsoleKey.Enter, ConsoleKey.Spacebar };

        public static int Run(int menuLength, params Screen[] screens)
        {
            SelectedIndex = 0;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();

                Write(screens);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (UpKeys.Contains(keyPressed))
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                        SelectedIndex = menuLength - 1;
                }

                if (DownKeys.Contains(keyPressed))
                {
                    SelectedIndex++;
                    if (SelectedIndex == menuLength)
                        SelectedIndex = 0;
                }
            } while (!ChoiceKeys.Contains(keyPressed));
            return SelectedIndex;
        }

        public static void Write(params Screen[] screens)
        {
            var result = new List<string>[screens.Length];
            for (int i = 0; i < screens.Length; i++)
                result[i] = screens[i].CreateScreen();

            for (int i = 0; i < result.Length - 1; i++)
            {
                if (result[i].Count < result[i + 1].Count)
                    EqualizeScreensHeight(result[i], result[i + 1].Count);
                else if (result[i].Count > result[i + 1].Count)
                    EqualizeScreensHeight(result[i + 1], result[i].Count);
            }


            for (int i = 0; i < result[0].Count; i++)
            {
                StringBuilder line = new();
                for (int j = 0; j < screens.Length; j++)
                    line.Append((result[j][i] + ' ').ToString());
                Console.WriteLine(line);
            }
        }

        public static void EqualizeScreensHeight(List<string> screen, int height)
        {
            var screenEnd = screen[screen.Count - 1];
            screen[screen.Count - 1] = VerticalBorder + new string(' ', screenEnd.Length - 2) + VerticalBorder;

            while (screen.Count != height - 1)
                screen.Add(VerticalBorder + new string(' ', screenEnd.Length - 2) + VerticalBorder);
            screen.Add(screenEnd);
        }

        public static void CreateTitle(List<string> screen, string text, int width)
        {
            var boarder = AngleBorder + new string(HorizontalBorder, width - 2) + AngleBorder;
            screen.Add(boarder);
            screen.Add(CreateJustifyString(text, width));
            screen.Add(boarder);
        }

        public static string CreateJustifyString(string text, int width)
        {
            var freeSpace = width - text.Length - 2;
            int leftSpace;
            int rightSpace;
            if (freeSpace % 2 == 0)
                leftSpace = rightSpace = freeSpace / 2;
            else
            {
                leftSpace = freeSpace / 2 + 1;
                rightSpace = freeSpace / 2;
            }
            return VerticalBorder + new string(' ', leftSpace) + text + new string(' ', rightSpace) + VerticalBorder;
        }

        public static string CreateBorder(int count)
        {
            var border = AngleBorder + new string(HorizontalBorder, Card.Width - 2) + AngleBorder;
            if (count == 1)
                return border;
            var borders = new string[count];
            for (int i = 0; i < count; i++)
                borders[i] = border;
            return String.Join(' ', borders);
        }

        public virtual List<string> CreateScreen()
        {
            return new List<string>();
        }
    }

    public class MenuScreen : Screen
    {
        public string Title;
        public List<string> Menu;

        public MenuScreen(string title, List<string> menu)
        {
            Title = title;
            Menu = menu;
        }

        public override List<string> CreateScreen()
        {
            var width = Card.Width + 6;
            var mainMenuScreen = new List<string>();
            var emptyLine = VerticalBorder + new string(' ', width - 2) + VerticalBorder;
            var buttonBorder = VerticalBorder + "  " + CreateBorder(1) + "  " + VerticalBorder;

            CreateTitle(mainMenuScreen, Title, width);
            mainMenuScreen.Add(emptyLine);

            for (int i = 0; i < Menu.Count; i++)
            {
                mainMenuScreen.Add(buttonBorder);
                if (SelectedIndex == i)
                    mainMenuScreen.Add(string.Format("{0} {1}{2}  {0}", VerticalBorder, SelectedIndexChar, CreateJustifyString(Menu[i].ToString(), Card.Width)));
                else
                    mainMenuScreen.Add(string.Format("{0}  {1}  {0}", VerticalBorder, CreateJustifyString(Menu[i].ToString(), Card.Width)));

                mainMenuScreen.Add(buttonBorder);
                mainMenuScreen.Add(emptyLine);
            }

            mainMenuScreen.Add(AngleBorder + new string(HorizontalBorder, width - 2) + AngleBorder);
            return mainMenuScreen;
        }
    }

    public class PlayerScreen : Screen
    {
        Player Player;
        string[] Move;

        public PlayerScreen(Player player, string[] move)
        {
            Player = player;
            Move = move;
        }

        public override List<string> CreateScreen()
        {
            var playerScreen = new List<string>();
            var width = string.Format("{0}      {1}      {0}", VerticalBorder, Player.Hand[0].ToString()).Length;
            var emptyLine = VerticalBorder + new string(' ', width - 2) + VerticalBorder;
            var cardBorder = VerticalBorder + "      " + CreateBorder(1) + "      " + VerticalBorder;

            CreateTitle(playerScreen, Player.NickName + ", " + Move[0], width);
            playerScreen.Add(emptyLine);
            playerScreen.Add(cardBorder);
            playerScreen.Add(string.Format("{0}      {1}      {0}", VerticalBorder, CreateJustifyString("Рука", Card.Width)));
            playerScreen.Add(cardBorder);
            playerScreen.Add(emptyLine);

            for (int i = 0; i < Player.Hand.Count; i++)
            {
                playerScreen.Add(cardBorder);
                if (SelectedIndex == i)
                    playerScreen.Add(string.Format("{0}     {1}{2}      {0}", VerticalBorder, SelectedIndexChar, Player.Hand[i].ToString()));
                else
                    playerScreen.Add(string.Format("{0}      {1}      {0}", VerticalBorder, Player.Hand[i].ToString()));
                playerScreen.Add(cardBorder);
                playerScreen.Add(emptyLine);
            }

            playerScreen.Add(cardBorder);
            if (SelectedIndex == Player.Hand.Count)
                playerScreen.Add(string.Format("{0}     {1}{2}      {0}", VerticalBorder, SelectedIndexChar, CreateJustifyString(Move[1], Card.Width)));
            else
                playerScreen.Add(string.Format("{0}      {1}      {0}", VerticalBorder, CreateJustifyString(Move[1], Card.Width)));
            playerScreen.Add(cardBorder);

            playerScreen.Add(AngleBorder + new string(HorizontalBorder, width - 2) + AngleBorder);
            return playerScreen;
        }
    }

    public class TableScreen : Screen
    {
        Table Table;

        public TableScreen(Table table)
        {
            Table = table;
        }

        public override List<string> CreateScreen()
        {
            var width = Card.Width * 2 + 5;
            var tableScreen = new List<string>();
            var emptyLine = VerticalBorder + new string(' ', width - 2) + VerticalBorder;
            var cardsBorder = VerticalBorder + " " + CreateBorder(2) + ' ' + VerticalBorder;

            CreateTitle(tableScreen, "Стол", width);
            tableScreen.Add(emptyLine);
            tableScreen.Add(cardsBorder);
            tableScreen.Add(string.Format("{0} {1} {2} {0}", VerticalBorder, CreateJustifyString("Атака", Card.Width),
                                                                             CreateJustifyString("Защита", Card.Width)));
            tableScreen.Add(cardsBorder);
            tableScreen.Add(emptyLine);

            for (int i = 0; i < Table.AttackCards.Length; i++)
            {
                tableScreen.Add(cardsBorder);
                tableScreen.Add(string.Format("{0} {1} {2} {0}", VerticalBorder, GetCard(Table.AttackCards[i]),
                                                                                 GetCard(Table.DefendCards[i])));
                tableScreen.Add(cardsBorder);
                tableScreen.Add(emptyLine);
            }

            tableScreen.Add(VerticalBorder + " " + AngleBorder + new string(HorizontalBorder, width - 6) + AngleBorder + ' ' + VerticalBorder);
            tableScreen.Add(string.Format("{0} {1} {0}", VerticalBorder, CreateJustifyString("Козырь - " + Table.Deck.Trumb, width - 4)));
            tableScreen.Add(VerticalBorder + " " + AngleBorder + new string(HorizontalBorder, width - 6) + AngleBorder + ' ' + VerticalBorder);
            tableScreen.Add(emptyLine);

            tableScreen.Add(AngleBorder + new string(HorizontalBorder, width - 2) + AngleBorder);
            return tableScreen;
        }

        static string GetCard(Card card)
        {
            if (card == null)
                return '|' + new string(' ', Card.Width - 2) + '|';
            return card.ToString();
        }
    }
}

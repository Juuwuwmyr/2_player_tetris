using System;
using System.IO;
using System.Media;
using System.Text;
using Tertris_2_palyer;

namespace Tetris_2_Player
{
    internal class Program
    {
        private static SoundPlayer backgroundMusic;

        static void ShowLandingPage()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string musicPath = Path.Combine(baseDir, "asset", "background.wav");

            if (File.Exists(musicPath))
            {
                backgroundMusic = new SoundPlayer(musicPath);
                backgroundMusic.PlayLooping();
            }

            const int starCount = 100;
            (int x, int y, bool visible)[] stars = new (int, int, bool)[starCount];
            Random rand = new Random();

            for (int i = 0; i < starCount; i++)
            {
                stars[i] = (
                    rand.Next(0, Console.WindowWidth),
                    rand.Next(0, Console.WindowHeight),
                    rand.NextDouble() > 0.5
                );
            }

            string[] cloudArt = new string[]
        {
    "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⣿⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⣿⡏⣿⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠤⣤⣤⣤⣤⣤⣤⣤⣤⣿⣿⠇⠀⢿⣿⣷⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⡶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠖⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠂⠀⠀⠀⠀",
    "⠀⠀⠘⢿⣿⣿⣟⠛⠛⠛⠛⠀⠀⠀⠛⠛⠛┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠈⠛⣿⣿⣦⡀⠀⠀⠀⠀⠀ ⠀┃  ███████████ ██████████ ███████████ ███████████   █████  █████████  ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⠀⢹⣿⡿⠀⠀⠀⠀⠀⠀⠀┃ ░█░░░███░░░█░░███░░░░░█░█░░░███░░░█░░███░░░░░███ ░░███  ███░░░░░███ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⠀⣾⣿⠁⢀⣤⣾⣦⡀ ⠀┃ ░   ░███  ░  ░███  █ ░ ░   ░███  ░  ░███    ░███  ░███ ░███    ░░░  ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⠀⣸⣿⢇⣶⣿⠟⠙⠻⣿ ⠀┃     ░███     ░██████       ░███     ░██████████   ░███ ░░█████████  ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⢠⣿⣿⠿⠋⠁⠀⠀⠀ ⠳⣄┃     ░███     ░███░░█       ░███     ░███░░░░░███  ░███  ░░░░░░░░███ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "⠀⠀⠀⠀⠀⡿⠋⠁⠀⠀⠀⠀⠀⠀⠀  ┃     ░███     ░███ ░   █    ░███     ░███    ░███  ░███  ███    ░███ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
    "                 ┃     █████    ██████████    █████    █████   █████ █████░░█████████  ┃ " ,
    "                 ┃     ░░░░░    ░░░░░░░░░░    ░░░░░    ░░░░░   ░░░░░ ░░░░░  ░░░░░░░░░  ┃ " ,
    "                 ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛              ",
    "                  ━━━━━━━━━━━━━━━━━━━━━━━━━BSIS 2 PRODUCTION━━━━━━━━━━━━━━━━━━━━━━━━━━━━                            "
 
        };

            int cloudStartX = 10;
            int cloudStartY = 1;
            int protectedTop = cloudStartY;
            int protectedBottom = cloudStartY + cloudArt.Length;

            string[] menuItems = { "START GAME", "HIGH SCORE", "EXIT" };
            int selectedIndex = 0;
            int previousIndex = -1;
            int menuStartY = 19;

            DrawStaticUI(cloudArt, cloudStartX, cloudStartY);
            DrawMenu(menuItems, selectedIndex, menuStartY);

            while (true)
            {
                for (int i = 0; i < starCount; i++)
                {
                    if (stars[i].y >= protectedTop && stars[i].y <= protectedBottom)
                        continue;

                    Console.SetCursorPosition(stars[i].x, stars[i].y);
                    Console.Write(" ");

                    if (rand.NextDouble() < 0.3)
                        stars[i].visible = !stars[i].visible;

                    Console.SetCursorPosition(stars[i].x, stars[i].y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(stars[i].visible ? "⭑" : " ");
                }
                Console.ResetColor();


                if (previousIndex != selectedIndex)
                {
                    DrawMenu(menuItems, selectedIndex, menuStartY);
                    previousIndex = selectedIndex;
                }

                System.Threading.Thread.Sleep(120);

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
                            break;

                        case ConsoleKey.DownArrow:
                            selectedIndex = (selectedIndex + 1) % menuItems.Length;
                            break;

                        case ConsoleKey.Enter:
                            if (selectedIndex == 0)
                                return;

                            if (selectedIndex == 1)
                            {
                                ShowHighScore();

                                Console.Clear();
                                DrawStaticUI(cloudArt, cloudStartX, cloudStartY);
                                DrawMenu(menuItems, selectedIndex, menuStartY);
                                previousIndex = -1; 
                            }

                            if (selectedIndex == 2)
                                Environment.Exit(0);

                            break;

                    }
                }
            }
        }

        static void DrawStaticUI(string[] cloudArt, int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < cloudArt.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(cloudArt[i]);
            }
            Console.ResetColor();
        }

        static void DrawMenu(string[] menuItems, int selectedIndex, int menuStartY)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                int width = menuItems[i].Length + 4;
                int startX = (Console.WindowWidth - width) / 2;
                int y = menuStartY + i * 3;

                Console.SetCursorPosition(startX, y);
                Console.Write(new string(' ', width));
                Console.SetCursorPosition(startX, y + 1);
                Console.Write(new string(' ', width));
                Console.SetCursorPosition(startX, y + 2);
                Console.Write(new string(' ', width));

                if (i == selectedIndex)
                {
                    Console.SetCursorPosition(startX, y);
                    Console.Write("┏" + new string('━', width - 2) + "┓");
                    Console.SetCursorPosition(startX, y + 1);
                    Console.Write("┃ " + menuItems[i] + " ┃");
                    Console.SetCursorPosition(startX, y + 2);
                    Console.Write("┗" + new string('━', width - 2) + "┛");
                }
                else
                {
                    Console.SetCursorPosition(startX + 2, y + 1);
                    Console.Write(menuItems[i]);
                }
            }
        }

        static void ShowHighScore()
        {
            Console.Clear();

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "highscores.txt"
            );

            var scores = HighScore.LoadFromFile(path);

            Console.WriteLine("==== HIGH SCORES ====\n");

            if (scores.Count == 0)
                Console.WriteLine("No high scores found.");
            else
                for (int i = 0; i < Math.Min(5, scores.Count); i++)
                    Console.WriteLine($"{i + 1}. {scores[i]}");

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);

            Console.Clear();
            return;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            Console.Title = "2-Player Tetris Game";

            try
            {
                Console.SetWindowSize(120, 45);
                Console.SetBufferSize(120, 45);
            }
            catch { }

            ShowLandingPage();

            Game game = new Game();
            game.Run();
        }
    }
}

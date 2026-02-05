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

            const int starCount = 30;
            (int x, int y, bool visible)[] stars = new (int, int, bool)[starCount];
            Random rand = new Random();

            for (int i = 0; i < starCount; i++)
            {
                stars[i] = (rand.Next(0, Console.WindowWidth), rand.Next(0, Console.WindowHeight), rand.NextDouble() > 0.5);
            }
            string[] cloudArt = new string[]
            {
        "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⣿⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⣿⡏⣿⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠤⣤⣤⣤⣤⣤⣤⣤⣤⣿⣿⠇⠀⢿⣿⣷⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⣶⡶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠶⠖⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠂⠀⠀⠀⠀",
        "⠀⠀⠘⢿⣿⣿⣟⠛⠛⠛⠛⠀⠀⠀⠛⠛⠛⠛⠋⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠈⠛⣿⣿⣦⡀⠀⠀⠀⠀⠀ ⠀┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⠀⢹⣿⡿⠀⠀⠀⠀⠀ ⠀┃ ████████╗ ███████╗ ████████╗ ██████╗  ██╗  ███████╗ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⠀⣾⣿⠁⢀⣤⣾⣦⡀ ⠀┃ ╚══██╔══╝ ██╔════╝ ╚══██╔══╝ ██╔══██╗ ██║  ██╔════╝ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⠀⣸⣿⢇⣶⣿⠟⠙⠻⣿ ⠀┃    ██║    █████╗      ██║    ██████╔╝ ██║  ███████╗ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⢠⣿⣿⠿⠋⠁⠀⠀⠀ ⠳⣄┃    ██║    ██╔══╝      ██║    ██╔═██║  ██║  ╚════██║ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "⠀⠀⠀⠀⠀⡿⠋⠁⠀⠀⠀⠀⠀⠀⠀  ┃    ██║    ███████╗    ██║    ██║ ██║  ██║  ███████║ ┃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
        "                 ┃    ╚═╝    ╚══════╝    ╚═╝    ╚═╝ ╚═╝  ╚═╝  ╚══════╝ ┃",
        "                 ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛                                  ",
        "                 ━━━━━━━━━━━━━━━━━━━BSIS 2 PRODUCTION━━━━━━━━━━━━━━━━━━━"
            };

            int cloudStartX = 17;
            int cloudStartY = 1;

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < cloudArt.Length; i++)
            {
                Console.SetCursorPosition(cloudStartX, cloudStartY + i);
                Console.Write(cloudArt[i]);
            }

            int cloudWidth = 0;
            foreach (string line in cloudArt)
            {
                if (line.Length > cloudWidth)
                    cloudWidth = line.Length;
            }

            Console.ResetColor();

            string[] menuItems = { "START GAME", "HIGH SCORE", "EXIT" };
            int selectedIndex = 0;
            int menuStartY = 15 + 2;

            while (true)
            {
                Console.Clear();
                for (int i = 0; i < starCount; i++)
                {
                    if (rand.NextDouble() < 0.3)
                        stars[i].visible = !stars[i].visible;

                    Console.SetCursorPosition(stars[i].x, stars[i].y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(stars[i].visible ? "⭑" : " ");
                }

                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < cloudArt.Length; i++)
                {
                    Console.SetCursorPosition(cloudStartX, cloudStartY + i);
                    Console.Write(cloudArt[i]);
                }
               


                for (int i = 0; i < menuItems.Length; i++)
                {
                    int menuWidth = menuItems[i].Length + 4;
                    int menuHeight = 3;
                    int startX = (Console.WindowWidth - menuWidth) / 2;
                    int startYMenu = menuStartY + i * 3;

                    for (int y = 0; y < menuHeight; y++)
                    {
                        Console.SetCursorPosition(startX, startYMenu + y);
                        Console.Write(new string(' ', menuWidth));
                    }
                }

                for (int i = 0; i < menuItems.Length; i++)
                {
                    int menuWidth = menuItems[i].Length + 4;
                    int startX = (Console.WindowWidth - menuWidth) / 2;
                    int startYMenu = menuStartY + i * 3;

                    if (i == selectedIndex)
                    {
                        Console.SetCursorPosition(startX, startYMenu);
                        Console.Write("┏" + new string('━', menuWidth - 2) + "┓");

                        Console.SetCursorPosition(startX, startYMenu + 1);
                        Console.Write("┃ " + menuItems[i].PadRight(menuWidth - 4) + " ┃");

                        Console.SetCursorPosition(startX, startYMenu + 2);
                        Console.Write("┗" + new string('━', menuWidth - 2) + "┛");
                    }
                    else
                    {
                        Console.SetCursorPosition(startX + 2, startYMenu + 1);
                        Console.Write(menuItems[i]);
                    }
                }

                System.Threading.Thread.Sleep(150);

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedIndex--;
                            if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            selectedIndex++;
                            if (selectedIndex >= menuItems.Length) selectedIndex = 0;
                            break;
                        case ConsoleKey.Enter:
                            switch (selectedIndex)
                            {
                                case 0: return;
                                case 1: ShowHighScore(); break;
                                case 2: Environment.Exit(0); break;
                            }
                            break;
                    }
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
            {
                Console.WriteLine("No high scores found.");
            }
            else
            {
                for (int i = 0; i < Math.Min(5, scores.Count); i++)
                {
                    Console.WriteLine($"{i + 1}. {scores[i]}");
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);
        }
        static void Main(string[] args)
            {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            Console.Write("\u001b[?25l"); 
            Console.Write("\u001b[2J");  

            Console.Title = "2-Player Tetris Game";

                try
                {
                    int maxWidth = Console.LargestWindowWidth;
                    int maxHeight = Console.LargestWindowHeight;

                    Console.SetWindowSize(Math.Min(maxWidth, 120), Math.Min(maxHeight, 50));
                    Console.SetBufferSize(Math.Min(maxWidth, 120), Math.Min(maxHeight, 50));
                }
                catch
                {
                    Console.WindowWidth = 70;
                    Console.WindowHeight = 45;
                    Console.BufferWidth = 70;
                    Console.BufferHeight = 45;
                }

                ShowLandingPage();

                Game game = new Game();
                game.Run();
            }
        }
    }


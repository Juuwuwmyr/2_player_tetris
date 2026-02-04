using System;
using System.IO;
using System.Media;
using Tertris_2_palyer;

namespace Tetris_2_Player
{
    internal class Program
    {
        private static SoundPlayer backgroundMusic;

        static void ShowLandingPage()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Load and play music
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string musicPath = Path.Combine(baseDir, "asset", "background.wav");

            if (File.Exists(musicPath))
            {
                backgroundMusic = new SoundPlayer(musicPath);
                backgroundMusic.PlayLooping();
            }

            Random random = new Random();

            string[] cloudArt = new string[]
            {
                "           ⣀⣠⣤⣤⣄⡀⠀⠀⠀⠀⠀⠀⣀⣀⣀⡀     ",
                "        ⣠⢞⡫⠄⠀⠀⠀⠉⠓⢦⡀⢀⣴⡿⠉⠀⠉⠉⠳⢦⡀      ",
                "    ⣀⣠⣄⣀⠀⣼⢣⣎⠀⠀⠀⠀⠀⠀⠀⢻⣟⣟⠔⠀   ⢀⠹⡄      ",
                "  ⢠⠿⢩⣄⡀⠈⠻⠇⡎⡎⡂⠀⠀⠀⠀⠀⢸⣐⣈⡀⠀    ⠀⡄⡆⣿    ",
                " ⣠⡤⠶⣾⠀⠘⠉⣻⠄⠀⠀⠀⠀⠀⡴⠏⢉⠈⢻⡄⠀⠀    ⠃⢇⡿⣟⠿⠳⣦⡀ ",
                " ⢠⡞⠉⠀⠀⠈⠳⠶⠶⠋⠀⠀⠀⠀⠸⠁⠀⢿⣤⡾⠁⠀⠀⠀⠁   ⠀⠀⠈⣧ ",
                " ⢠⡟⠸⢠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀     ⠀⠀⠀⢤ ",
                " ⢷⡸⡙⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀     ⠀⠀⢧ ",
                " ⢀⡴⠞⠛⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀     ⠀⣇ ",
                " ⣰⡏⠁⢀⣤⣤⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀     ⠀⠀⠙ ",
                " ⣿⢇⠀⣿⡀⠀⢹⡆⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⠀⠀⠀⠀⠀⠀⠀   ⠀⠀⠀ ",
                " ⠘⢯⣓⡀⠀⢀⣼⣱⡀⠀⠀⠀⠀⠀⠀⣰⠟⠁⢙⡧⠀⠀⠀⠀⢠⢀⠀⠀⠀⠀ ",
                " ⠈⠉⠉⠙⣧⠹⣌⠀⠀⠀⠀⠀⠀⢿⡀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀          "
            };

            Console.ForegroundColor = ConsoleColor.Yellow;
            int numberOfClouds = 5;
            for (int i = 0; i < numberOfClouds; i++)
            {
                int cloudX = random.Next(0, Math.Max(1, Console.WindowWidth - cloudArt[0].Length));
                int cloudY = random.Next(2, 9);
                for (int j = 0; j < cloudArt.Length && cloudY + j < Console.WindowHeight; j++)
                {
                    Console.SetCursorPosition(cloudX, cloudY + j);
                    Console.Write(cloudArt[j]);
                }
            }

            Console.ResetColor();
            string[] tetrisArt = new string[]
            {
                "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓",
                "┃ ████████╗ ███████╗ ████████╗ ██████╗  ██╗  ███████╗ ┃",
                "┃ ╚══██╔══╝ ██╔════╝ ╚══██╔══╝ ██╔══██╗ ██║  ██╔════╝ ┃",
                "  ┃    ██║    █████╗      ██║    ██████╔╝ ██║  ███████╗ ┃  ",
                "  ┃    ██║    ██╔══╝      ██║    ██╔═██║  ██║  ╚════██║ ┃  ",
                "┃    ██║    ███████╗    ██║    ██║ ██║  ██║  ███████║ ┃",
                "┃    ╚═╝    ╚══════╝    ╚═╝    ╚═╝ ╚═╝  ╚═╝  ╚══════╝ ┃",
                "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛",
                "━━━━━━━━━━━━━━━━━━━BSIS 2 PRODUCTION━━━━━━━━━━━━━━━━━━━"
            };

            int startY = (30 - tetrisArt.Length) / 2;
            foreach (string line in tetrisArt)
            {
                int startX = (Console.WindowWidth - line.Length) / 2;
                Console.SetCursorPosition(startX, startY++);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(line);
            }

            string[] instructionBox = new string[]
              {
                "┏━━━━━━━━━━━━━┓",
                "┃ PRESS ENTER ┃",
                "┗━━━━━━━━━━━━━┛"
              };

            int instructionY = startY + 2; // position below title
            foreach (string line in instructionBox)
            {
                int instructionX = (Console.WindowWidth - line.Length) / 2;
                Console.SetCursorPosition(instructionX, instructionY++);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(line);
            }

            Console.ResetColor();

            // Wait for Enter
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
            static void Main(string[] args)
            {
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


using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace Tertris_2_palyer
{
    public class Game
    {
        private SoundPlayer backgroundMusic;
        private Player player1;
        private Player player2;
        private bool gameOver;
        private bool paused;
        private Random random;

        public const int BOARD_WIDTH = 12;
        public const int BOARD_HEIGHT = 25;
        public const int INITIAL_HP = 100;
        public const int INITIAL_GAME_SPEED = 70;
        public const int MIN_GAME_SPEED = 70;
        public const int SPEED_DECREMENT = 20;
        public const int LINES_PER_LEVEL = 5;

        private int currentGameSpeed;
        private int totalLinesCleared;
        private Stack<string> gameHistory;
        private List<HighScore> highScores;

        public Game()
        {
            random = new Random();
            Console.Clear();
            Console.CursorVisible = true;

            Console.Write("Enter Player 1 name: ");
            string p1Name = Console.ReadLine();

            Console.Write("Enter Player 2 name: ");
            string p2Name = Console.ReadLine();

            Console.CursorVisible = false;

            player1 = new Player(p1Name, 5, 2, random);
            player2 = new Player(p2Name, 79, 2, random);

            gameOver = false;
            paused = false;
            currentGameSpeed = INITIAL_GAME_SPEED;
            totalLinesCleared = 0;
            gameHistory = new Stack<string>();
            highScores = new List<HighScore>();
           
        }

        public void Run()
        {
            Console.Clear();
            Console.CursorVisible = false;

            while (!gameOver)
            {
                if (!paused)
                {
                    Update();
                }

                Render();
                HandleInput();

                System.Threading.Thread.Sleep(currentGameSpeed);
            }

            HandleGameOver();
        }

        private void Update()
        {
            player1.Update();
            player2.Update();

            int p1Lines = player1.ClearLines();
            int p2Lines = player2.ClearLines();

            if (p1Lines > 0)
            {
                player2.TakeDamage(20 * p1Lines);
            }

            if (p2Lines > 0)
            {
                player1.TakeDamage(20 * p2Lines);
            }





            totalLinesCleared = (player1.Score + player2.Score) / 100;

            int newSpeed = INITIAL_GAME_SPEED - (totalLinesCleared / LINES_PER_LEVEL) * SPEED_DECREMENT;
            currentGameSpeed = Math.Max(newSpeed, MIN_GAME_SPEED);

            if (player1.IsGameOver() || player2.IsGameOver())
            {
                gameOver = true;
            }
        }

        private void Render()
        {
            Console.SetCursorPosition(0, 5);

            player1.Render();
            player2.Render();


        }

        private void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.W:
                        player1.RotatePiece();
                        break;
                    case ConsoleKey.A:
                        player1.MovePiece(-1, 0);
                        break;
                    case ConsoleKey.S:
                        player1.MovePiece(0, 1);
                        break;
                    case ConsoleKey.D:
                        player1.MovePiece(1, 0);
                        break;

                    case ConsoleKey.UpArrow:
                        player2.RotatePiece();
                        break;
                    case ConsoleKey.LeftArrow:
                        player2.MovePiece(-1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        player2.MovePiece(0, 1);
                        break;
                    case ConsoleKey.RightArrow:
                        player2.MovePiece(1, 0);
                        break;

                    case ConsoleKey.P:
                        paused = !paused;
                        break;
                    case ConsoleKey.Escape:
                        gameOver = true;
                        break;
                }
            }
        }

        private void HandleGameOver()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "highscores.txt");

            highScores = HighScore.LoadFromFile(filePath);

            if (player1.Score > 0)
                highScores.Add(new HighScore(player1.Name, player1.Score, DateTime.Now));

            if (player2.Score > 0)
                highScores.Add(new HighScore(player2.Name, player2.Score, DateTime.Now));

            highScores.Sort();
            if (highScores.Count > 10)
                highScores = highScores.GetRange(0, 10);

            HighScore.SaveToFile(filePath, highScores);

            Console.Clear();

            Console.Clear();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string musicPath = Path.Combine(baseDir, "asset", "gameover.wav");
            if (File.Exists(musicPath))
            {
                backgroundMusic = new SoundPlayer(musicPath);
                backgroundMusic.Play();
            }
            string gameOverArt = @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣤⣤⣤⣤⣄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⣠⣤⡄⠀⠀⠀⠀⠀⢀⡴⣩⣤⡶⣿⡿⢿⣿⣧⣿⣦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⣧⣴⣿⠀⠀⠀⠀⢠⠟⣦⣿⣆⣙⣏⣴⣦⣽⠿⢧⣿⣷⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⡇⠳⢾⠀⠀⠀⠀⢸⣼⡋⠀⠀⠉⠉⠉⠉⠀⠀⠀⠈⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⣇⣠⣄⠀⠀⠀⠀⢸⣿⣦⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⣻⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢁⠘⠉⡇⠀⠀⠀⢀⣿⡏⠀⣀⣤⣤⣰⠄⢀⣤⣶⣶⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢠⠒⢻⠀⠀⡗⠢⣄⠀⢸⣿⣧⠈⠛⠛⠿⠋⢄⣿⠉⠉⠉⠹⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⣼⠶⣾⠀⠰⣿⠒⢾⣿⣮⣿⣽⣦⠀⠀⢀⣠⡄⢸⣷⡀⠀⢀⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢀⡜⢹⡄⠿⠆⠀⠀⠀⢠⠷⣌⡇⠙⣯⠑⣤⣿⣀⡩⠿⢿⣿⠂⣼⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢸⠁⢸⠟⠂⠸⡀⠀⠀⢈⠀⠸⣇⣴⣿⢦⡈⠀⠙⣷⣞⣿⡟⢀⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⡇⠉⠀⠀⠀⠃⠀⠀⠉⠀⠀⣿⣿⣿⣄⠙⢤⡀⠈⠛⠉⢀⣼⠁⠀⢀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠹⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣷⡄⠈⠓⠒⣲⠿⣼⣿⣷⣼⣿⣷⣤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⠑⢄⠀⠀⠀⠀⠀⠀⣼⣿⣿⣿⣿⣿⣿⣦⣠⡴⠿⣾⣽⣽⡇⠀⠘⣿⣿⣿⣆⠀⠀⠀⠀⠀⣀⣀⣠⣤⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢱⡆⠀⠀⣈⠴⢋⣿⣿⣿⣿⣿⣿⣏⠋⠀⠀⠈⠙⣾⢧⡀⠀⢹⣿⣿⣿⣦⣶⣶⢿⣛⣛⣉⠉⢹⡇⠀
⠀⠀⠀⠀⠀⠀⠀⣠⠾⠓⠒⢉⣠⣴⣿⣿⣿⣿⣿⣿⣿⣿⣧⣀⣠⣤⣼⡷⡿⡿⠛⠛⣻⣿⣿⣿⠿⠿⠹⣿⡛⢻⣷⠘⣿⠀
⠀⠀⠀⠀⠀⠀⢸⣤⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣟⠛⠋⢉⣽⣷⣶⡄⢿⣆⠀⣿⡇⢿⣇⣀⡀⠀⢻⣷⢿⣿⡄⢻⡇
⠀⠀⠀⠀⣀⣀⣸⣿⣿⣿⡿⠿⣿⠛⠋⢩⣯⢹⣿⡿⠟⠋⠀⠀⠸⣿⠀⢹⣿⠈⢿⣆⣿⡇⠸⣿⠋⠁⠀⠸⣿⡀⢹⣷⠸⣷
⣶⡶⠿⢛⣛⣉⠉⢁⣼⣷⣶⡄⢿⣷⣤⣾⣿⡄⣿⣦⣤⡄⠀⠀⠀⣿⡆⠀⣿⡆⠈⢿⣿⡇⠀⢿⠷⠿⠟⢀⣙⣡⣤⣤⡶⠿
⢸⣧⢠⣿⠛⢻⣷⢸⣿⠀⢹⣷⢸⣿⠻⠟⢹⣇⢸⣿⠁⣀⣀⠀⠀⠘⠿⠶⠟⠁⣀⣈⣉⣤⣤⡶⠶⠾⠛⠛⠋⠉⠁⠀⠀⠀
⠈⣿⠈⣿⡄⣤⣶⡄⣿⡿⠛⣿⡆⣿⡆⠀⠘⣿⠌⠿⠟⢛⣋⣠⣤⣴⣶⠶⠿⠛⠛⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢹⡇⠹⣧⣨⣿⠇⠸⡧⠀⠙⣇⣘⣡⣤⣤⣶⠶⠿⠛⠛⠉⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠘⣿⣀⣈⣩⣥⣴⡶⠿⠿⠛⠛⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀              ⠙⠛⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀";
            string[] artLines = gameOverArt.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);


            int consoleWidth = Console.WindowWidth;

            int startY = 1; 
            for (int i = 0; i < artLines.Length; i++)
            {
                int x = Math.Max(0, (consoleWidth - artLines[i].Length) / 2);
                Console.SetCursorPosition(x, startY + i);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(artLines[i]);
            }

            Console.ResetColor();

            startY += artLines.Length + 2;

            string winnerText;
            if (player1.IsGameOver() && player2.IsGameOver())
            {
                winnerText = "It's a tie!";
            }
            else if (player1.IsGameOver())
            {
                winnerText = $"{player2.Name} wins!";
            }
            else
            {
                winnerText = $"{player1.Name} wins!";
            }


            Console.SetCursorPosition((consoleWidth - winnerText.Length) / 2, startY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(winnerText);




         
            Console.SetCursorPosition((consoleWidth - "Press any key to exit".Length) / 2, startY + highScores.Count + 2);
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }

    }
}

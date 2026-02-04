using System;
using System.Collections.Generic;

namespace Tertris_2_palyer
{
    public class Game
    {
        private Player player1;
        private Player player2;
        private bool gameOver;
        private bool paused;
        private Random random;

        public const int BOARD_WIDTH = 12;
        public const int BOARD_HEIGHT = 28;
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
            player1 = new Player(5, 2, random);
            player2 = new Player(48, 2, random);
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
                    case ConsoleKey.Q:
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
                    case ConsoleKey.N:
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
            if (player1.Score > 0)
            {
                highScores.Add(new HighScore("Player 1", player1.Score));
            }
            if (player2.Score > 0)
            {
                highScores.Add(new HighScore("Player 2", player2.Score));
            }

            highScores.Sort();

            Console.SetCursorPosition(15, 12);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("GAME OVER!");

            Console.SetCursorPosition(15, 14);
            if (player1.IsGameOver() && player2.IsGameOver())
            {
                Console.Write("It's a tie!");
            }
            else if (player1.IsGameOver())
            {
                Console.Write("Player 2 wins!");
            }
            else
            {
                Console.Write("Player 1 wins!");
            }
            Console.SetCursorPosition(15, 16);
            Console.Write("High Scores:");
            for (int i = 0; i < Math.Min(5, highScores.Count); i++)
            {
                Console.SetCursorPosition(15, 18 + i);
                Console.Write($"{i + 1}. {highScores[i]}");
            }

            Console.SetCursorPosition(15, 25);
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

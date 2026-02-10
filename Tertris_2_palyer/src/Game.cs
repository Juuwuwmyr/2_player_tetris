using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using Tetris_2_palyer;

namespace Tertris_2_palyer
{
    public class Game
    {
        private ScreenBuffer screen;

        private Stopwatch gameTimer;
        private long lastFallTimeP1;
        private long lastFallTimeP2;
        private SoundPlayer backgroundMusic;
        private Player player1;
        private Player player2;
        private bool gameOver;
        private bool paused;
        private Random random;
        private bool softDropP1;
        private bool softDropP2;
        private long softDropP1Until;
        private long softDropP2Until;

        public const int BOARD_WIDTH = 12;
        public const int BOARD_HEIGHT = 25;
        public const int INITIAL_HP = 300;
        public const int INITIAL_GAME_SPEED = 500;
        public const int MIN_GAME_SPEED = 70;
        public const int SPEED_DECREMENT = 20;
        public const int LINES_PER_LEVEL = 5;
        public const int SOFT_DROP_SPEED = 50;
        public const int SOFT_DROP_TTL = 300;

        private int currentGameSpeed;
        private int totalLinesCleared;
        private Stack<string> gameHistory;
        private List<HighScore> highScores;

        public Game()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.SetWindowSize(160, 40);
            Console.SetBufferSize(160, 40);

            screen = new ScreenBuffer(Console.WindowWidth, Console.WindowHeight);
            random = new Random();
            Console.Clear();
            Console.CursorVisible = true;

            string p1Name = ReadValidNameAscii("PLAYER ONE");
            string p2Name = ReadValidNameAscii("PLAYER TWO");

            player1 = new Player(p1Name, 5, 2, random);
            player2 = new Player(p2Name, 74, 2, random);




            Console.Clear();
            Console.CursorVisible = false;

            player1 = new Player(p1Name, 5, 2, random);
            player2 = new Player(p2Name, 74, 2, random);

            gameTimer = Stopwatch.StartNew();
            lastFallTimeP1 = 0;
            lastFallTimeP2 = 0;
            gameOver = false;
            paused = false;
            currentGameSpeed = INITIAL_GAME_SPEED;
            totalLinesCleared = 0;
            gameHistory = new Stack<string>();
            highScores = new List<HighScore>();
            softDropP1 = false;
            softDropP2 = false;
            softDropP1Until = 0;
            softDropP2Until = 0;
        }
        private string ReadValidNameAscii(string title)
        {
            StringBuilder name = new StringBuilder();

            while (true)
            {
                Console.Clear();

                DrawAsciiCentered(title, 6, ConsoleColor.Cyan);
                DrawAsciiCentered("ENTER NAME", 10, ConsoleColor.Yellow);

                Console.SetCursorPosition(45, 14);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("A–Z ONLY   5–10 CHARACTERS");
                Console.ResetColor();

                if (name.Length > 0)
                {
                    var art = AsciiFont.Render(name.ToString());
                    int startX = 30;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(startX, 18);
                    Console.Write(art[0]);
                    Console.SetCursorPosition(startX, 19);
                    Console.Write(art[1]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (IsValidName(name.ToString()))
                        return name.ToString().ToUpper();

                    DrawAsciiCentered("INVALID NAME!", 23, ConsoleColor.Red);
                    Thread.Sleep(1200);
                    name.Clear();
                    continue;
                }

                if (key.Key == ConsoleKey.Backspace && name.Length > 0)
                {
                    name.Length--;
                    continue;
                }

                char c = char.ToUpper(key.KeyChar);

                if (c >= 'A' && c <= 'Z' && name.Length < 10)
                {
                    name.Append(c);
                }
            }
        }

        private void DrawAsciiCentered(string text, int y, ConsoleColor color)
        {
            var art = AsciiFont.Render(text);
            int startX = 30;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(startX, y);
            Console.Write(art[0]);
            Console.SetCursorPosition(startX, y + 1);
            Console.Write(art[1]);
            Console.ResetColor();
        }
        private bool IsValidName(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (input.Length < 5 || input.Length > 10)
                return false;

            foreach (char c in input)
            {
                if (c < 'A' || c > 'Z')
                    return false;
            }

            return true;
        }


     

        public void Run()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string musicPath = Path.Combine(baseDir, "asset", "game.wav");
            if (File.Exists(musicPath))
            {
                backgroundMusic = new SoundPlayer(musicPath);
                backgroundMusic.PlayLooping();
            }
            while (!gameOver)
            {

                HandleInput();
                if (!paused)
                {
                    Update();
                }
                Render();
            }

            HandleGameOver(); 
        }


        private void Update()
        {
            long now = gameTimer.ElapsedMilliseconds;

            int speedP1 = (softDropP1 && now <= softDropP1Until) ? SOFT_DROP_SPEED : currentGameSpeed;
            int speedP2 = (softDropP2 && now <= softDropP2Until) ? SOFT_DROP_SPEED : currentGameSpeed;
            if (now - lastFallTimeP1 >= speedP1)
            {
                player1.MovePiece(0, 1);
                lastFallTimeP1 = now;
            }
            if (now - lastFallTimeP2 >= speedP2)
            {
                player2.MovePiece(0, 1);
                lastFallTimeP2 = now;
            }

            int p1Lines = player1.ClearLines();
            int p2Lines = player2.ClearLines();

            if (p1Lines > 0) player2.TakeDamage(20 * p1Lines);
            if (p2Lines > 0) player1.TakeDamage(20 * p2Lines);

            UpdateGameSpeed(p1Lines, p2Lines);

            if (player1.IsGameOver() || player2.IsGameOver())
                gameOver = true;
        }
        private void UpdateGameSpeed(int p1Lines, int p2Lines)
        {
            int linesThisTurn = p1Lines + p2Lines;
            if (linesThisTurn > 0)
            {
                totalLinesCleared += linesThisTurn;

                int newSpeed = INITIAL_GAME_SPEED - (totalLinesCleared / LINES_PER_LEVEL) * SPEED_DECREMENT;

                currentGameSpeed = Math.Max(newSpeed, MIN_GAME_SPEED);
            }
        }



        private void Render()
        {
            RenderCenterUI();

            player1.Render();
            player2.Render();

            if (paused)
            {
                int boxW = 18;
                int boxH = 5;
                int startX = (Console.WindowWidth - boxW) / 2;
                int startY = 10;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(startX, startY);
                Console.Write("┏" + new string('━', boxW - 2) + "┓");
                for (int i = 1; i < boxH - 1; i++)
                {
                    Console.SetCursorPosition(startX, startY + i);
                    Console.Write("┃" + new string(' ', boxW - 2) + "┃");
                }
                Console.SetCursorPosition(startX, startY + boxH - 1);
                Console.Write("┗" + new string('━', boxW - 2) + "┛");
                Console.SetCursorPosition(startX + 4, startY + 2);
                Console.Write("PAUSED");
                Console.ResetColor();
            }

        }
        private void RenderCenterUI()
        {
            string title = "TETRIS 2-PLAYER";
            int tx = 55;
            Console.SetCursorPosition(tx, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(title);
            Console.ResetColor();
            int midX = 60;
            for (int y = 2; y < Game.BOARD_HEIGHT + 4; y++)
            {
                Console.SetCursorPosition(midX, y);
                Console.Write("░");
            }
            Console.SetCursorPosition(midX - 1, 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("VS");
            Console.ResetColor();
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
                        softDropP1 = true;
                        softDropP1Until = gameTimer.ElapsedMilliseconds + SOFT_DROP_TTL;
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
                        softDropP2 = true;
                        softDropP2Until = gameTimer.ElapsedMilliseconds + SOFT_DROP_TTL;
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
        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀█████ █████ ⠀⠀⠀⠀⠀                  █████   ███   █████  ███      
░░███ ░░███                        ░░███   ░███  ░░███  ░░░             
 ░░███ ███    ██████  █████ ████    ░███   ░███   ░███  ████  ████████  
  ░░█████    ███░░███░░███ ░███     ░███   ░███   ░███ ░░███ ░░███░░███ 
   ░░███    ░███ ░███ ░███ ░███     ░░███  █████  ███   ░███  ░███ ░███ 
    ░███    ░███ ░███ ░███ ░███      ░░░█████░█████░    ░███  ░███ ░███ 
    █████   ░░██████  ░░████████       ░░███ ░░███      █████ ████ █████
   ░░░░░     ░░░░░░    ░░░░░░░░         ░░░   ░░░      ░░░░░ ░░░░ ░░░░░ ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀";
            string[] artLines = gameOverArt.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);


            int consoleWidth = Console.WindowWidth;

            int startY = 7; 
            for (int i = 0; i < artLines.Length; i++)
            {
                int x = Math.Max(0, (consoleWidth - artLines[i].Length) / 2);
                Console.SetCursorPosition(25, startY + i);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(artLines[i]);
            }
            string[] menuItems = { "  REPLAY   ", " EXIT GAME " };
            int selectedIndex = 0;
            int menuStartY = startY + 4; 

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


            string winnerRaw;

            if (player1.IsGameOver() && player2.IsGameOver())
                winnerRaw = "TIE!";
            else if (player1.IsGameOver())
                winnerRaw = player2.Name + " WINS!";
            else
                winnerRaw = player1.Name + " WINS!";

            var winArt = AsciiFont.Render(winnerRaw);

            int centerX = (Console.WindowWidth - winArt[0].Length) / 2;
            int winY = startY;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(25, winY);
            Console.Write(winArt[0]);

            Console.SetCursorPosition(25, winY + 1);
            Console.Write(winArt[1]);
            Console.ResetColor();

            Console.SetCursorPosition(45, startY + highScores.Count + 2);
            while (true)
            {
                for (int i = 0; i < menuItems.Length; i++)
                {
                    int menuWidth = menuItems[i].Length + 4;
                    int menuHeight = 3;
                    int startX = 55;
                    int startYMenu = 22 + i * 3;

                    for (int y = 0; y < menuHeight; y++)
                    {
                        Console.SetCursorPosition(startX, startYMenu + y);
                        Console.Write(new string(' ', menuWidth));
                    }
                }

                for (int i = 0; i < menuItems.Length; i++)
                {
                    int menuWidth = menuItems[i].Length + 4;
                    int startX = 55;
                    int startYMenu = 22 + i * 3;

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

                Thread.Sleep(120);

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
                            if (selectedIndex == 0)
                            {
                                Console.Clear();
                                backgroundMusic?.Stop();

                                Game newGame = new Game();
                                newGame.Run();
                                return;
                            }
                            else
                            {
                                Environment.Exit(0);
                            }

                            break;
                    }
                }
            }

        }

    }
}

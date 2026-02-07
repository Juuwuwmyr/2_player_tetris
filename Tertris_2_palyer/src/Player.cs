using System;
using System.Collections.Generic;
using System.Text;

namespace Tertris_2_palyer
{
    public class Player
    {
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Score { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private Board board;
        private Tetromino currentPiece;
        private Queue<TetrominoType> nextPieces;
        private Random random;

        private const int INFO_PADDING = 3;

        public Player(string name, int x, int y, Random rand)
        {
            Name = name;
            X = x;
            Y = y;
            HP = Game.INITIAL_HP;
            Score = 0;
            random = rand;

            board = new Board();
            nextPieces = new Queue<TetrominoType>();

            for (int i = 0; i < 5; i++)
                nextPieces.Enqueue((TetrominoType)random.Next(7));

            SpawnNewPiece();
        }

        public void Update()
        {
            MovePiece(0, 1);
        }

        public void Render()
        {
            board.Render(X, Y);
            currentPiece.Render(X, Y);
            RenderInfo();
        }

        private void RenderInfo()
        {

            int infoX = X + Game.BOARD_WIDTH * 2 + INFO_PADDING;
            int infoY = Y;
            int infoBoxX = infoX;
            int infoBoxY = infoY;
            int infoBoxWidth = 18;
            int infoBoxHeight = 6;

            Console.ForegroundColor = ConsoleColor.White;
            DrawBox(infoBoxX, infoBoxY, infoBoxWidth, infoBoxHeight);
            Console.SetCursorPosition(infoBoxX + 2, infoBoxY + 1);
            Console.Write(Name);

            int barUnits = 12;
            int filled = (int)Math.Round(barUnits * Math.Max(0, Math.Min(Game.INITIAL_HP, HP)) / (double)Game.INITIAL_HP);
            ConsoleColor hpColor = ConsoleColor.Red;
            double pct = HP / (double)Game.INITIAL_HP;
            if (pct >= 0.7) hpColor = ConsoleColor.Green;
            else if (pct >= 0.3) hpColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(infoBoxX + 2, infoBoxY + 2);
            Console.ForegroundColor = hpColor;
            Console.Write(new string('█', filled) + new string('░', barUnits - filled));
            Console.ResetColor();

            Console.SetCursorPosition(infoBoxX + 2, infoBoxY + 3);
            Console.Write($"Score: {Score}");


            Console.SetCursorPosition(infoX, infoBoxY + infoBoxHeight + 1);
            Console.Write("Next Moves:");

            int boxX = infoX;
            int boxY = infoBoxY + infoBoxHeight + 2;

            int boxWidth = 12;   
            int boxHeight = 14;  

            DrawBox(boxX, boxY, boxWidth, boxHeight);

            RenderNextPiece(boxX + 3, boxY + 1);

        }

        private void RenderNextPiece(int x, int y)
        {
            var preview = nextPieces.ToArray();

            for (int p = 0; p < Math.Min(3, preview.Length); p++)
            {
                Tetromino t = new Tetromino(preview[p]);
                int[,] shape = t.GetRotatedShape();

                for (int i = 0; i < Tetromino.SIZE; i++)
                {
                    Console.SetCursorPosition(x, y + p * 4 + i);

                    for (int j = 0; j < Tetromino.SIZE; j++)
                    {
                        if (shape[i, j] != 0)
                        {
                            Console.ForegroundColor = Tetromino.GetColor(t.Type);
                            Console.Write("▒▒");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                }
            }
            Console.ResetColor();
        }


        public void MovePiece(int dx, int dy)
        {
            int newX = currentPiece.X + dx;
            int newY = currentPiece.Y + dy;

            if (CheckCollision(currentPiece, newX, newY))
            {
                if (dy > 0)
                {
                    PlacePiece();
                    SpawnNewPiece();
                }
                return;
            }

            currentPiece.X = newX;
            currentPiece.Y = newY;
        }

        public void RotatePiece()
        {
            int newRotation = (currentPiece.Rotation + 1) % 4;

            int[] kickX = { 0, -1, 1, -2, 2 };
            int[] kickY = { 0, -1 };

            foreach (int dx in kickX)
            {
                foreach (int dy in kickY)
                {
                    Tetromino test = new Tetromino(currentPiece.Type)
                    {
                        X = currentPiece.X + dx,
                        Y = currentPiece.Y + dy,
                        Rotation = newRotation
                    };

                    if (!CheckCollision(test, test.X, test.Y))
                    {
                        currentPiece.X = test.X;
                        currentPiece.Y = test.Y;
                        currentPiece.Rotation = newRotation;
                        return;
                    }
                }
            }
        }

        private bool CheckCollision(Tetromino piece, int x, int y)
        {
            int[,] shape = piece.GetRotatedShape();

            for (int i = 0; i < Tetromino.SIZE; i++)
            {
                for (int j = 0; j < Tetromino.SIZE; j++)
                {
                    if (shape[i, j] == 0) continue;

                    int bx = x + j;
                    int by = y + i;

                    if (bx < 0 || bx >= Game.BOARD_WIDTH || by >= Game.BOARD_HEIGHT)
                        return true;

                    if (by >= 0 && board.GetCell(bx, by) != 0)
                        return true;
                }
            }
            return false;
        }

        private void PlacePiece()
        {
            int[,] shape = currentPiece.GetRotatedShape();

            for (int i = 0; i < Tetromino.SIZE; i++)
            {
                for (int j = 0; j < Tetromino.SIZE; j++)
                {
                    if (shape[i, j] == 0) continue;

                    int bx = currentPiece.X + j;
                    int by = currentPiece.Y + i;

                    if (bx >= 0 && bx < Game.BOARD_WIDTH &&
                        by >= 0 && by < Game.BOARD_HEIGHT)
                    {
                        board.SetCell(bx, by, (int)currentPiece.Type + 1);
                    }
                }
            }
        }

        public int ClearLines()
        {
            int lines = board.ClearFullLines();
            if (lines > 0)
            {
                switch (lines)
                {
                    case 1: Score += 40; break;
                    case 2: Score += 100; break;
                    case 3: Score += 300; break;
                    case 4: Score += 1200; break;
                    default: Score += lines * 300; break; 
                }
            }
            return lines;
        }
        private void DrawBox(int x, int y, int width, int height)
        {
            Console.ForegroundColor = ConsoleColor.White;

          
            Console.SetCursorPosition(x, y);
            Console.Write("┏" + new string('━', width - 2) + "┓");

           
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("┃" + new string(' ', width - 2) + "┃");
            }

           
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write("┗" + new string('━', width - 2) + "┛");

            Console.ResetColor();
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        private void SpawnNewPiece()
        {
            TetrominoType type = nextPieces.Dequeue();
            currentPiece = new Tetromino(type);
            nextPieces.Enqueue((TetrominoType)random.Next(7));

            if (CheckCollision(currentPiece, currentPiece.X, currentPiece.Y))
            {
                HP = 0;
            }
        }

        public bool IsGameOver() => HP <= 0;
    }
}

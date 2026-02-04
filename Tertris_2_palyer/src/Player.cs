using System;
using System.Collections.Generic;
using System.Text;
namespace Tertris_2_palyer
{
    public class Player
    {

        public int HP { get; private set; }
        public int Score { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private Board board;
        private Tetromino currentPiece;
        private Queue<TetrominoType> nextPieces;
        private Random random;

        private const int INFO_PADDING = 3;

        public Player(int x, int y, Random rand)
        {
            X = x;
            Y = y;
            HP = Game.INITIAL_HP;
            Score = 0;
            random = rand;

            board = new Board();
            nextPieces = new Queue<TetrominoType>();

            for (int i = 0; i < 5; i++)
            {
                nextPieces.Enqueue((TetrominoType)random.Next(7));
            }

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

    Console.ForegroundColor = ConsoleColor.White;

    Console.SetCursorPosition(infoX, infoY);
    Console.Write($"HP: {HP,-5}");


    Console.SetCursorPosition(infoX, infoY + 1);
    Console.Write($"Score: {Score}");

    Console.SetCursorPosition(infoX, infoY + 3);
    Console.Write("Sunod na Move:");

    RenderNextPiece(infoX, infoY + 4);
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

                        Console.OutputEncoding = Encoding.Unicode;
                        if (shape[i, j] != 0)
                        {
                            Console.ForegroundColor = Tetromino.GetColor(t.Type);
                            Console.Write("▒▒");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write("  ");
                        }


                    }
                }
            }
        }


        public void MovePiece(int dx, int dy)
        {
            int newX = currentPiece.X + dx;
            int newY = currentPiece.Y + dy;

            if (CheckCollision(newX, newY))
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
            Tetromino temp = new Tetromino(currentPiece.Type)
            {
                X = currentPiece.X,
                Y = currentPiece.Y,
                Rotation = (currentPiece.Rotation + 1) % 4
            };

            if (!CheckCollision(temp.X, temp.Y))
            {
                currentPiece.Rotation = temp.Rotation;
            }
        }

        private bool CheckCollision(int x, int y)
        {
            int[,] shape = currentPiece.GetRotatedShape();

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
                Score += lines * 100;
            }
            return lines;
        }




        public void TakeDamage(int damage)
        {
            HP =  HP - damage;
        }


        private void SpawnNewPiece()
        {
            TetrominoType type = nextPieces.Dequeue();
            currentPiece = new Tetromino(type);
            nextPieces.Enqueue((TetrominoType)random.Next(7));

            if (CheckCollision(currentPiece.X, currentPiece.Y))
            {
                HP = 0;
            }
        }

        public bool IsGameOver() => HP <= 0;
    }
}

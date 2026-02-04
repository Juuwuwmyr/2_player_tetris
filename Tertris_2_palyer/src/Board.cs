using System;
using System.Text;

namespace Tertris_2_palyer
{
    public class Board
    {
        private int[,] cells;
        
        public Board()
        {
            cells = new int[Game.BOARD_HEIGHT, Game.BOARD_WIDTH];
            InitializeBoard();
        }
        
        private void InitializeBoard()
        {
            for (int i = 0; i < Game.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < Game.BOARD_WIDTH; j++)
                {
                    cells[i, j] = 0;
                }
            }
        }
        
        public void Render(int offsetX, int offsetY)
        {
            DrawBorder(offsetX - 1, offsetY - 1, Game.BOARD_WIDTH * 2 + 2, Game.BOARD_HEIGHT + 2);

            for (int i = 0; i < Game.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < Game.BOARD_WIDTH; j++)
                {
                    Console.SetCursorPosition(offsetX + j * 2, offsetY + i);

                    int cell = cells[i, j];

                    if (cell == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        TetrominoType type = (TetrominoType)(cell - 1);
                        Console.OutputEncoding = Encoding.Unicode;
                        Console.ForegroundColor = Tetromino.GetColor(type);
                        Console.Write("▒▒");
                        Console.ResetColor();
                    }
                }
            }

        }

        public int GetCell(int x, int y)
        {
            if (x >= 0 && x < Game.BOARD_WIDTH && y >= 0 && y < Game.BOARD_HEIGHT)
            {
                return cells[y, x];
            }
            return -1;  
        }
        
        public void SetCell(int x, int y, int value)
        {
            if (x >= 0 && x < Game.BOARD_WIDTH && y >= 0 && y < Game.BOARD_HEIGHT)
            {
                cells[y, x] = value;
            }
        }
        
        public int ClearFullLines()
        {
            int linesCleared = 0;
            
            for (int i = Game.BOARD_HEIGHT - 1; i >= 0; i--)
            {
                bool isFull = true;
                
                for (int j = 0; j < Game.BOARD_WIDTH; j++)
                {
                    if (cells[i, j] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                
                if (isFull)
                {
                    MoveLinesDown(i);
                    linesCleared++;
                    i++;  
                }
            }
            
            return linesCleared;
        }
        
        private void MoveLinesDown(int startLine)
        {
            for (int i = startLine; i > 0; i--)
            {
                for (int j = 0; j < Game.BOARD_WIDTH; j++)
                {
                    cells[i, j] = cells[i - 1, j];
                }
            }
            for (int j = 0; j < Game.BOARD_WIDTH; j++)
            {
                cells[0, j] = 0;
            }
        }
        
        private void DrawBorder(int x, int y, int width, int height)
        {
          
            Console.SetCursorPosition(x, y);
            Console.Write("┏");
            for (int i = 0; i < width - 2; i++)
            {
                Console.Write("━");
            }
            Console.Write("┓");
            
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("┃");
                Console.SetCursorPosition(x + width - 1, y + i);
                Console.Write("┃");
            }
            
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write("┗");
            for (int i = 0; i < width - 2; i++)
            {
                Console.Write("━");
            }
            Console.Write("┛");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
namespace Tertris_2_palyer
{
    

    public enum TetrominoType
    {
        I = 0,  
        O,      
        T,      
        S,     
        Z,     
        J,    
        L       
    }
    
    public class Tetromino
    {


        public const int SIZE = 4;
        public TetrominoType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; set; }
        
        private static readonly int[,,] shapes = {
        
            {
                {0, 0, 0, 0},
                {1, 1, 1, 1},
                {0, 0, 0, 0},
                {0, 0, 0, 0}
            },
           
            {
                {0, 0, 0, 0},
                {0, 1, 1, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0}
            },
            {
                {0, 0, 0, 0},
                {0, 1, 0, 0},
                {1, 1, 1, 0},
                {0, 0, 0, 0}
            },
            {
                {0, 0, 0, 0},
                {0, 1, 1, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0}
            },
            {
                {0, 0, 0, 0},
                {1, 1, 0, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0}
            },
            {
                {0, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 1, 1, 0},
                {0, 0, 0, 0}
            },
            {
                {0, 0, 0, 0},
                {0, 0, 1, 0},
                {1, 1, 1, 0},
                {0, 0, 0, 0}
            }
        };
        
        public Tetromino(TetrominoType type)
        {
            Type = type;
            Rotation = 0;
            X = Game.BOARD_WIDTH / 2 - 2;
            Y = 0;
        }
        
        public int[,] GetRotatedShape()
        {
            int[,] rotatedShape = new int[SIZE, SIZE];
            
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    rotatedShape[i, j] = shapes[(int)Type, i, j];
                }
            }
            
            for (int r = 0; r < Rotation; r++)
            {
                int[,] temp = new int[SIZE, SIZE];
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        temp[j, SIZE - 1 - i] = rotatedShape[i, j];
                    }
                }
                
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        rotatedShape[i, j] = temp[i, j];
                    }
                }
            }
            
            return rotatedShape;
        }
        public void Render(int offsetX, int offsetY)
        {
            int[,] shape = GetRotatedShape();
            
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (shape[i, j] != 0)
                    {

                        Console.OutputEncoding = Encoding.Unicode;
                        Console.SetCursorPosition(offsetX + (X + j) * 2, offsetY + Y + i);
                        Console.ForegroundColor = GetColor(Type);
                        Console.Write("▒▒");
                        Console.ResetColor();
                    }
                }
            }
        }
        private static readonly Dictionary<TetrominoType, ConsoleColor> Colors =
              new Dictionary<TetrominoType, ConsoleColor>
          {
            { TetrominoType.I, ConsoleColor.Cyan },
            { TetrominoType.O, ConsoleColor.Yellow },
            { TetrominoType.T, ConsoleColor.Magenta },
            { TetrominoType.S, ConsoleColor.Green },
            { TetrominoType.Z, ConsoleColor.Red },
            { TetrominoType.J, ConsoleColor.Blue },
            { TetrominoType.L, ConsoleColor.DarkYellow }
          };

        public static ConsoleColor GetColor(TetrominoType type)
        {
            return Colors[type];
        }

        public void RenderToBuffer(StringBuilder buffer, int offsetX, int offsetY)
        {
            int[,] shape = GetRotatedShape();

            for (int i = 0; i < SIZE; i++)
            {
                buffer.Append($"\u001b[{offsetY + Y + i + 1};{(offsetX + X) * 2 + 1}H");

                for (int j = 0; j < SIZE; j++)
                {
                    buffer.Append(shape[i, j] != 0 ? "▒▒" : "  ");
                }
            }
        }

        public void RenderPreview(int offsetX, int offsetY)
        {
            int[,] shape = GetRotatedShape();
            
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (shape[i, j] != 0)
                    {

                        Console.OutputEncoding = Encoding.Unicode;
                        Console.SetCursorPosition(offsetX + j * 2, offsetY + i);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("▒▒");
                    }
                    else
                    {
                        Console.SetCursorPosition(offsetX + j * 2, offsetY + i);
                        Console.Write("  ");
                    }
                }
            }
        }
    }
}
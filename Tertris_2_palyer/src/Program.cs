using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tertris_2_palyer
{
    internal class Program
    {
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
                Console.WindowWidth = 100;
                Console.WindowHeight = 45;
                Console.BufferWidth = 100;
                Console.BufferHeight = 45;
            }
            
            Game game = new Game();
            game.Run();
        }
    }
}

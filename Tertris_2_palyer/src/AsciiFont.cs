using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris_2_palyer
{ 
    public static class AsciiFont
    {
        public static readonly Dictionary<char, string[]> Font = new Dictionary<char, string[]>
        {
            ['A'] = new[]
            {
            " ▄▀▄ ",
            " █▀█ "
        },
            ['B'] = new[]
            {
            " ██▄ ",
            " █▄█ "
        },
            ['C'] = new[]
            {
            " ▄▀▀ ",
            " ▀▄▄ "
        },
            ['D'] = new[]
            {
            " █▀▄ ",
            " █▄▀ "
        },
            ['E'] = new[]
            {
            " ██▀ ",
            " █▄▄ "
        },
            ['F'] = new[]
            {
            " █▀ ",
            " █▀ "
        },
            ['G'] = new[]
            {
            " ▄▀  ",
            " ▀▄█ "
        },
            ['H'] = new[]
            {
            " █▄█ ",
            " █ █ "
        },
            ['I'] = new[]
            {
            " █ ",
            " █ "
        },
            ['J'] = new[]
            {
            "   █ ",
            " ▀▄█ "
        },
            ['K'] = new[]
            {
            " █▄▀ ",
            " █ █ "
        },
            ['L'] = new[]
            {
            " █   ",
            " █▄▄ "
        },
            ['M'] = new[]
            {
            "█▄ ▄█",
            "█ ▀ █"
        },
            ['N'] = new[]
            {
            " █▄ █ ",
            " █ ▀█ "
        },
            ['O'] = new[]
            {
            " ▄▀▄ ",
            " ▀▄▀ "
        },
            ['R'] = new[]
            {
            " █▀▄ ",
            " █▀▄ "
        },
            ['S'] = new[]
            {
            " ▄▀▀ ",
            " ▄██ "
        },
            ['T'] = new[]
            {
            " ▀█▀ ",
            "  █  "
        },
            ['W'] = new[]
            {
            "█ █ █",
            "▀▄▀▄▀"
        },
            ['!'] = new[]
            {
            " ▀ ",
            " ▀ "
        },
            ['P'] = new[]
        {
            " █▀█ ",
            " █▀▀ "
        },
                    ['Q'] = new[]
        {
            " ▄▀▄ ",
            " ▀▄█▄ "
        },
                    ['U'] = new[]
        {
            " █ █ ",
            " ▀▄█ "
        },
    
            ['V'] = new[]
        {
            " █ █ ",
            " ▀▄▀ "
        },
                    ['X'] = new[]
        {
            " ▀▄▀ ",
            " ▄▀▄ "
        },
                    ['Y'] = new[]
        {
            " █ █ ",
            "  █  "
        },
                    ['Z'] = new[]
        {
            "▀█▀ ",
            "█▄▄ "
        },
            [' '] = new[]
            {
            "   ",
            "   "
        }
        };

        public static string[] Render(string text)
        {
            text = text.ToUpper();
            var lines = new string[2] { "", "" };

            foreach (char c in text)
            {
                if (!Font.ContainsKey(c)) continue;

                lines[0] += Font[c][0] + " ";
                lines[1] += Font[c][1] + " ";
            }

            return lines;
        }
    }

}


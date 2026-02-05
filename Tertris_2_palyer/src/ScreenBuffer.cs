using System.Text;
using System;

public class ScreenBuffer
{
    private readonly int width;
    private readonly int height;
    private readonly char[,] buffer;

    public ScreenBuffer(int width, int height)
    {
        this.width = width;
        this.height = height;
        buffer = new char[height, width];
        Clear();
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                buffer[y, x] = ' ';
    }

    public void Set(int x, int y, char c)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return;
        buffer[y, x] = c;
    }

    public void Draw()
    {
        Console.SetCursorPosition(0, 0);
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                sb.Append(buffer[y, x]);
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }
}

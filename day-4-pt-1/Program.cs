using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string[] grid;

        try
        {
            using StreamReader reader = new("input.txt");
            string text = reader.ReadToEnd();

            grid = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
            return;
        }

        string word = "XMAS";
        int count = CountOccurrences(grid, word);
        Console.WriteLine(count);
    }

    static int CountOccurrences(string[] grid, string word)
    {
        int count = 0;
        int rows = grid.Length;
        int cols = grid[0].Length;

        // Directions: right, down, diagonal down-right, left, up, diagonal up-left, diagonal down-left, diagonal up-right
        int[,] directions = new int[,] {
            { 0, 1 },   // right
            { 1, 0 },   // down
            { 1, 1 },   // diagonal down-right
            { 0, -1 },  // left
            { -1, 0 },  // up
            { -1, -1 }, // diagonal up-left
            { 1, -1 },  // diagonal down-left
            { -1, 1 }   // diagonal up-right
        };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                for (int d = 0; d < directions.GetLength(0); d++)
                {
                    int rowDir = directions[d, 0];
                    int colDir = directions[d, 1];
                    count += SearchWord(grid, word, r, c, rowDir, colDir);
                }
            }
        }

        return count;
    }

    static int SearchWord(string[] grid, string word, int startRow, int startCol, int rowDir, int colDir)
    {
        int length = word.Length;
        int rows = grid.Length;
        int cols = grid[0].Length;

        int r = startRow;
        int c = startCol;

        for (int i = 0; i < length; i++)
        {
            // Check boundaries
            if (r < 0 || r >= rows || c < 0 || c >= cols || grid[r][c] != word[i])
                return 0;
            r += rowDir;
            c += colDir;
        }

        return 1; // Found the word
    }
}

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

        int count = CountXMasOccurrences(grid);
        Console.WriteLine(count);
    }

    static int CountXMasOccurrences(string[] grid)
    {
        int count = 0;
        int rows = grid.Length;
        int cols = grid[0].Length;

        for (int r = 1; r < rows - 1; r++)
        {
            for (int c = 1; c < cols - 1; c++)
            {
                // Center must be 'A'
                if (grid[r][c] != 'A')
                    continue;

                // Check all possible combinations of MAS patterns
                if (IsValidXMasPattern(grid, r, c))
                {
                    count++;
                }
            }
        }

        return count;
    }

    static bool IsValidXMasPattern(string[] grid, int row, int col)
    {
        // Check all possible combinations of MAS and SAM in X pattern
        return CheckDiagonalPair(grid, row, col, 'M', 'A', 'S') || // MAS/MAS
               CheckDiagonalPair(grid, row, col, 'S', 'A', 'M') || // SAM/SAM
               CheckDiagonalPair(grid, row, col, 'M', 'A', 'S', true) || // MAS/SAM
               CheckDiagonalPair(grid, row, col, 'S', 'A', 'M', true);   // SAM/MAS
    }

    static bool CheckDiagonalPair(string[] grid, int row, int col, char start, char middle, char end, bool mixedDirection = false)
    {
        // For the first diagonal (top-left to bottom-right)
        bool firstDiagonal = grid[row - 1][col - 1] == start &&
                            grid[row][col] == middle &&
                            grid[row + 1][col + 1] == end;

        // For the second diagonal (top-right to bottom-left)
        bool secondDiagonal;
        if (mixedDirection)
        {
            // If mixed direction, check for reversed pattern in second diagonal
            secondDiagonal = grid[row - 1][col + 1] == end &&
                            grid[row][col] == middle &&
                            grid[row + 1][col - 1] == start;
        }
        else
        {
            // If same direction, check for same pattern in second diagonal
            secondDiagonal = grid[row - 1][col + 1] == start &&
                            grid[row][col] == middle &&
                            grid[row + 1][col - 1] == end;
        }

        return firstDiagonal && secondDiagonal;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string input = File.ReadAllText("input.txt").Trim();
        var diskDefrag = new DiskDefragmenter(input);
        long checksum = diskDefrag.CompactAndCalculateChecksum();
        Console.WriteLine($"Filesystem checksum: {checksum}");
    }
}

class DiskDefragmenter
{
    private readonly List<int> diskMap;
    private List<Block> blocks;

    public DiskDefragmenter(string input)
    {
        diskMap = input.Select(c => int.Parse(c.ToString())).ToList();
        InitializeBlocks();
    }

    private void InitializeBlocks()
    {
        blocks = new List<Block>();
        int currentPosition = 0;
        int fileId = 0;
        bool isFile = true;

        foreach (int length in diskMap)
        {
            if (isFile)
            {
                blocks.Add(new Block { IsFile = true, Length = length, FileId = fileId, Position = currentPosition });
                fileId++;
            }
            else
            {
                blocks.Add(new Block { IsFile = false, Length = length, Position = currentPosition });
            }
            currentPosition += length;
            isFile = !isFile;
        }
    }

    public long CompactAndCalculateChecksum()
    {
        int totalLength = blocks.Sum(b => b.Length);
        bool[] disk = new bool[totalLength];
        int[] fileIds = new int[totalLength];

        // Initialize initial state
        int position = 0;
        foreach (var block in blocks)
        {
            if (block.IsFile)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    disk[position + i] = true;
                    fileIds[position + i] = block.FileId;
                }
            }
            position += block.Length;
        }

        // Get list of files sorted by ID in descending order
        var files = blocks.Where(b => b.IsFile)
                         .OrderByDescending(b => b.FileId)
                         .ToList();

        // Try to move each file once
        foreach (var file in files)
        {
            // Find the current position of the file
            int currentStart = -1;
            for (int i = 0; i < totalLength && currentStart == -1; i++)
            {
                if (disk[i] && fileIds[i] == file.FileId)
                {
                    currentStart = i;
                }
            }

            // Find the leftmost suitable free space
            int bestTarget = -1;
            int currentPos = 0;

            while (currentPos < currentStart)
            {
                // Check if we have enough consecutive free space
                bool canFit = true;
                for (int i = 0; i < file.Length && currentPos + i < currentStart; i++)
                {
                    if (disk[currentPos + i])
                    {
                        canFit = false;
                        break;
                    }
                }

                if (canFit && currentPos + file.Length <= currentStart)
                {
                    bestTarget = currentPos;
                    break;
                }

                currentPos++;
            }

            // If we found a suitable position, move the file
            if (bestTarget != -1)
            {
                // Clear old position
                for (int i = 0; i < file.Length; i++)
                {
                    disk[currentStart + i] = false;
                    fileIds[currentStart + i] = 0;
                }

                // Set new position
                for (int i = 0; i < file.Length; i++)
                {
                    disk[bestTarget + i] = true;
                    fileIds[bestTarget + i] = file.FileId;
                }
            }
        }

        // Calculate checksum
        long checksum = 0;
        for (int i = 0; i < totalLength; i++)
        {
            if (disk[i])
            {
                checksum += (long)i * fileIds[i];
            }
        }

        return checksum;
    }
}

class Block
{
    public bool IsFile { get; set; }
    public int Length { get; set; }
    public int FileId { get; set; }
    public int Position { get; set; }
}

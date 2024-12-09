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
        var compactedBlocks = new List<int>();
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

        // Compact files
        for (int i = totalLength - 1; i >= 0; i--)
        {
            if (disk[i]) // Found a file block
            {
                int fileId = fileIds[i];
                // Find leftmost free space
                int targetPos = 0;
                while (targetPos < i && disk[targetPos])
                {
                    targetPos++;
                }
                
                if (targetPos < i)
                {
                    // Move block
                    disk[targetPos] = true;
                    disk[i] = false;
                    fileIds[targetPos] = fileId;
                    fileIds[i] = 0;
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

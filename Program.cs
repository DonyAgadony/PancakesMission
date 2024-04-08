using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] inputFile = File.ReadAllLines("message.txt");
        List<string> input = new List<string>(inputFile);
        (int x, int y) start = (-1, -1);
        (int x, int y) end = (-1, -1);
        List<List<char>> map = new List<List<char>>();
        Dictionary<(int, int), int> distanceCosts = new Dictionary<(int, int), int>();

        void ExplorePaths(int x, int y)
        {
            int currentCost = distanceCosts[(x, y)];

            void NeighbourCost(int newX, int newY)
            {
                if (newX < 0 || newX >= map[0].Count || newY < 0 || newY >= map.Count)
                    return;

                if (map[newY][newX] + 1 >= map[y][x])
                {
                    if (!distanceCosts.ContainsKey((newX, newY)) || distanceCosts[(newX, newY)] > currentCost + 1)
                    {
                        distanceCosts[(newX, newY)] = currentCost + 1;
                        ExplorePaths(newX, newY);
                    }
                }
            }

            NeighbourCost(x + 1, y);
            NeighbourCost(x - 1, y);
            NeighbourCost(x, y + 1);
            NeighbourCost(x, y - 1);
        }

        foreach (var line in input)
        {
            var charList = new List<char>();
            foreach (char c in line)
            {
                if (c == 'S')
                {
                    start = (charList.Count, map.Count);
                    charList.Add('a');
                }
                else if (c == 'E')
                {
                    end = (charList.Count, map.Count);
                    distanceCosts[end] = 0;
                    charList.Add('z');
                }
                else
                    charList.Add(c);
            }
            map.Add(charList);
        }

        ExplorePaths(end.x, end.y);


        void FindShortestPathFromAToEnd()
        {
            var startInA = new List<(int, int)>();
            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[row].Count; col++)
                {
                    if (map[row][col] == 'a')
                        startInA.Add((col, row));
                }
            }

            int lowest = int.MaxValue;
            foreach (var coord in startInA)
            {
                if (!distanceCosts.ContainsKey(coord))
                    continue;
                var count = distanceCosts[coord];
                if (count < lowest)
                    lowest = count;
            }

            Console.WriteLine(lowest);
        }

        FindShortestPathFromAToEnd();
    }
}
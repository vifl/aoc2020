using System;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day02 : Day
    {
        public override int DayNumber => 2;

        public override string SolvePartOne()
        {
            var validCount = 0;
            var lines = GetPuzzleInput();
            foreach (var line in lines)
            {
                var parts = line.Split(" :-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(parts[0]);
                var max = int.Parse(parts[1]);
                var letter = parts[2][0];
                var password = parts[3];

                var letterCount = password.ToCharArray().Count(c => c == letter);
                if (letterCount >= min && letterCount <= max)
                    validCount++;
            }
            return validCount.ToString();
        }

        public override string SolvePartTwo()
        {
            var validCount = 0;
            var lines = GetPuzzleInput();
            foreach (var line in lines)
            {

                var parts = line.Split(" :-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var pos1 = int.Parse(parts[0]);
                var pos2 = int.Parse(parts[1]);
                var letter = parts[2][0];
                var password = parts[3];

                var count = 0;
                count += password[pos1 - 1] == letter ? 1 : 0;
                count += password[pos2 - 1] == letter ? 1 : 0;
                if (count == 1)
                    validCount++;
            }

            return validCount.ToString();
        }
    }
}
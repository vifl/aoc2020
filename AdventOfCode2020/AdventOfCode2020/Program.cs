using System;

namespace AdventOfCode2020
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Day day = new Day04();
            Console.WriteLine($"Day {day.DayNumber} part one: {day.SolvePartOne()}");
            Console.WriteLine($"Day {day.DayNumber} part two: {day.SolvePartTwo()}");
        }
    }
}
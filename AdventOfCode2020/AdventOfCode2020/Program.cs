using System;
using System.Runtime.Remoting;

namespace AdventOfCode2020
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var typeName = "AdventOfCode2020.Day" + DateTime.Now.Day.ToString("00");
            var day = (Day) Activator.CreateInstance("AdventOfCode2020", typeName).Unwrap();
            if (day == null)
            {
                Console.WriteLine("No implementation available for today");
                return;
            }

            Console.WriteLine($"Day {day.DayNumber} part one: {day.SolvePartOne()}");
            Console.WriteLine($"Day {day.DayNumber} part two: {day.SolvePartTwo()}");
        }
    }
}
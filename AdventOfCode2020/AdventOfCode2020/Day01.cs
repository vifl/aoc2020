using System;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day01 : Day
    {
        public override int DayNumber => 1;

        private int[] GetNumbers() => GetPuzzleInput().Select(line => int.Parse(line)).ToArray();

        public override string SolvePartOne()
        {
            var numbers = GetNumbers();
            foreach (var number1 in numbers)
            {
                var number2 = numbers.FirstOrDefault(n => n + number1 == 2020);
                if (number1 + number2 == 2020)
                {
                    return $"{number1} * {number2} = {number1 * number2}";
                }
            }
            throw new Exception("oops");
        }

        public override string SolvePartTwo()
        {
            var numbers = GetNumbers();
            foreach (var number0 in numbers)
            {
                foreach (var number1 in numbers)
                {
                    var number2 = numbers.FirstOrDefault(n => n + number1 + number0 == 2020);
                    if (number0 + number1 + number2 == 2020)
                    {
                        return $"{number0} * {number1} * {number2} = {number0 * number1 * number2}";
                    }
                }
            }
            throw new Exception("oops");
        }
    }
}
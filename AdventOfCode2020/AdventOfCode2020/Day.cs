using System.IO;

namespace AdventOfCode2020
{
    public abstract class Day
    {
        public abstract string SolvePartOne();
        public abstract string SolvePartTwo();

        public abstract int DayNumber { get; }
        private string DayNumberFormatted => DayNumber.ToString("D2");

        protected string[] GetPuzzleInput()
        {
            return File.ReadAllLines($"input{DayNumberFormatted}.txt");
        }
    }
}
using System.IO;

namespace AdventOfCode2020
{
    public abstract class Day
    {
        public abstract string SolvePartOne();

        public abstract string SolvePartTwo();

        public string DayNumber => this.GetType().Name.Substring(3);

        protected string[] GetPuzzleInput()
        {
            return File.ReadAllLines($"input{DayNumber}.txt");
        }
    }
}
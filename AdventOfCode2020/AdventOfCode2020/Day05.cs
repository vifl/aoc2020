using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day05 : Day
    {
        public override int DayNumber => 5;

        public override string SolvePartOne()
        {
            // var x = GetSeatId("FBFBBFFRLR"); // 44 * 8 + 5 = 357.
            // return "";
            var lines = GetPuzzleInput();
            var max = 0;
            foreach (var line in lines)
            {
                var id = GetSeatId(line);
                max = Math.Max(id, max);
            }
            return "" + max;
        }

        private int GetSeatId(string seatSpecification)
        {
            if (seatSpecification.Length != 10)
                throw new Exception("invalid seat specification length");

            var rowSpec = seatSpecification.Substring(0, 7);
            var rowBinary = rowSpec.Replace('F', '0').Replace('B', '1');
            int row = Convert.ToInt32(rowBinary, 2);

            var colSpec = seatSpecification.Substring(7, 3);
            var colBinary = colSpec.Replace('L', '0').Replace('R', '1');
            int col = Convert.ToInt32(colBinary, 2);

            return (row * 8) + col;

        }
        
        public override string SolvePartTwo()
        {
            var lines = GetPuzzleInput();
            var allSeatIds = lines.Select(GetSeatId).ToList();
            var lo = allSeatIds.Min();
            var hi = allSeatIds.Max();
            for (var i = lo; i < hi; i++)
            {
                if (allSeatIds.Contains(i))
                    continue;

                if (!allSeatIds.Contains(i - 1) || !allSeatIds.Contains(i + 1))
                    continue;
                
                return "" + i;
            }

            return "Not found!";
        }
    }
}
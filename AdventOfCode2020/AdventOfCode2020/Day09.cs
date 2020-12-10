using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day09 : Day
    {
        public override string SolvePartOne()
        {
             // var numbers = testdata.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(line => long.Parse(line)).ToList();
             // return "" + FindWeakness(numbers, 5);
            var numbers = GetPuzzleInput().Select(line => long.Parse(line)).ToList();
            return "" + FindWeakness(numbers, 25);
        }

        private long FindWeakness(List<long> numbers, int preambleLength)
        {
            for (var i = preambleLength; i < numbers.Count; i++)
            {
                if (!IsSumOfTwoInSeries(numbers[i], numbers.GetRange(i - preambleLength, preambleLength)))
                {
                    return numbers[i];
                }
            }

            throw new Exception("failed to find solution");
        }

        private bool IsSumOfTwoInSeries(long expectedNumber, List<long> range)
        {
            for (var i = 0; i < range.Count(); i++)
            {
                for (var j = i + 1; j < range.Count(); j++) // Suggested improvement after solution submitted. Changed from j = 0 to j = i + 1. Some more related code modified.
                {
                    if (range[i] + range[j] == expectedNumber)
                        return true;
                }
            }
            return false;
        }

        public override string SolvePartTwo()
        {
            // var numbers = testdata.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(line => long.Parse(line)).ToList();
            // var weakness = FindWeakness(numbers, 5);
            var numbers = GetPuzzleInput().Select(line => long.Parse(line)).ToList();
            var weakness = FindWeakness(numbers, 25);

             long? result = null;
             for (var start = 0; start < numbers.Count && result == null; start++)
             {
                 var sum = 0L;
                 for (var i = start; i < numbers.Count(); i++)
                 {
                     sum += numbers[i];
                     if (sum == weakness)
                     {
                         var range = numbers.GetRange(start, i - start);
                         
                         // match
                         result = range.Min() + range.Max();
                             
                         break;
                     }
                 }
             }

             return "" + result.Value;
        }
        
//         private string testdata = @"35
// 20
// 15
// 25
// 47
// 40
// 62
// 55
// 65
// 95
// 102
// 117
// 150
// 182
// 127
// 219
// 299
// 277
// 309
// 576";

    }
}

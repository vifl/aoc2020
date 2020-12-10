using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day10 : Day
    {
        public override string SolvePartOne()
        {

            //var adapterRatings = m_testdata.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(line => int.Parse(line)).OrderBy(joltage => joltage).ToList();
            var adapterRatings = GetPuzzleInput().Select(row => int.Parse(row)).OrderBy(joltage => joltage).ToList();

            adapterRatings.Add(adapterRatings.Max() + 3); // add device's built-in adapter
            var prev = 0;
            var differences = new Dictionary<int, int>();
            differences[1] = 0;
            differences[2] = 0;
            differences[3] = 0;
            foreach (var rating in adapterRatings)
            {
                var diff = rating - prev;
                if (diff == 0 || diff > 3)
                    throw new Exception("cannot find solution");
                differences[diff]++;
                prev = rating;
            }
            return "" + differences[1] * differences[3];
        }

        public override string SolvePartTwo()
        {
            //var adapterRatings = m_testdata.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(line => int.Parse(line)).OrderBy(joltage => joltage).ToList();
           
            var adapterRatings = GetPuzzleInput().Select(row => int.Parse(row)).OrderBy(joltage => joltage).ToList();

            adapterRatings.Insert(0, 0); // add charging outlet
            adapterRatings.Add(adapterRatings.Max() + 3); // add device's built-in adapter
            var arrangements = GetNumberOfArrangements(0, adapterRatings.ToArray());
            return "" + arrangements;
        }

        private readonly Dictionary<int, long> _cache = new Dictionary<int, long>();
        
        private long GetNumberOfArrangements(int index, int[] adapterRatings)
        {
            if (_cache.TryGetValue(index, out var val))
                return val;
            if (index == adapterRatings.Length - 1)
                return 1;
           
            long arrangements = 0;
            var i = index + 1;
            while (adapterRatings.Length > i && adapterRatings[i] - adapterRatings[index] <= 3)
            {
                arrangements += GetNumberOfArrangements(i, adapterRatings);
                i++;
            }

            _cache.Add(index, arrangements);
            return arrangements;
        }

//        private string m_testdata = @"16
//10
//15
//5
//1
//11
//7
//19
//6
//12
//4";
        
        
//        private string m_testdata = @"28
//33
//18
//42
//31
//14
//46
//20
//48
//47
//24
//23
//49
//45
//19
//38
//39
//11
//1
//32
//25
//35
//8
//17
//7
//9
//4
//2
//34
//10
//3";
    }
}

using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day06 : Day
    {
        private const string Letters = "abcdefghijklmnopqrstuvwxyz";

        public override string SolvePartOne()
        {
            var formData = GetFormData();
            var count = formData.Select(groupFormData => groupFormData.ToCharArray().Intersect(Letters).Distinct().Count()).Sum();
            return "" + count;
        }

        private string[] GetFormData()
        {
            var allText = File.ReadAllText($"input{DayNumber}.txt");
            return allText.Split(new[] {"\r\n\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }
        
        public override string SolvePartTwo()
        {
            var formData = GetFormData();
            var count = 0;
            
            foreach (var groupFormData in formData)
            {
                var expectedCount = groupFormData.Split(new []{"\r\n"}, StringSplitOptions.None).Length;
                var allMatching = groupFormData.ToCharArray().Select(c => c).Where(c => Letters.Contains(c)).GroupBy(c => c).Where(g => g.Count() == expectedCount);
                count += allMatching.Count();
            } 
            return "" + count;
        }
    }
}
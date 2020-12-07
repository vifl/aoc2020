using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day07 : Day
    {
        // result should be 4 for part 1
        // result should be 32 for part 2 
        private const string TestInput = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";

        // result should be 126 for part 2
        private const string TestInput2 = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";

        public override string SolvePartOne()
        {
            //var rules = TestInput.Split(new[] {"\r\n"}, StringSplitOptions.None);
            var rules = GetPuzzleInput();
            var parentColours = GetParentColours(rules);

            var listOfColours = GetPossibleTopParentBagColours("shiny gold", parentColours);
            
            return "" +  listOfColours.Distinct().Count();
        }

        private Dictionary<string, List<string>> GetParentColours(string [] rules)
        {
            var parents = new Dictionary<string, List<string>>();
            foreach (var rule in rules)
            {
                if (rule.Contains("contain no other bags"))
                    continue;
                var mainRegex = new Regex("^(.*) bags contain (.*)$");
                var mainRegexMatch = mainRegex.Match(rule);
                var mainBagColour = mainRegexMatch.Groups[1].Value;
                var subBags = mainRegexMatch.Groups[2].Value.Split(new []{", "}, StringSplitOptions.None);
                foreach (var subBag in subBags)
                {
                    var subRegex = new Regex("^(\\d*) (.*) bag");
                    var subMatch = subRegex.Match(subBag);
                    var subBagColour = subMatch.Groups[2].Value;
                    if (!parents.TryGetValue(subBagColour, out var parentBagColours))
                    {
                        parentBagColours = new List<string>();
                        parents.Add(subBagColour, parentBagColours);
                    }
                    parentBagColours.Add(mainBagColour);
                }
            }
            
            return parents;
        }

        private List<string> GetPossibleTopParentBagColours(string colour, Dictionary<string, List<string>> parentColours, int sanity = 0)
        {
            if (sanity > 1000)
            {
                throw new Exception("recursion stop");
            }
            var result = new List<string>();
            if (parentColours.TryGetValue(colour, out var parents))
            {
                result.AddRange(parents);
                foreach (var bagColour in parents)
                {
                    var grandParentColours = GetPossibleTopParentBagColours(bagColour, parentColours, sanity + 1);
                    result.AddRange(grandParentColours);
                }
            }
            return result;
        }

        public override string SolvePartTwo()
        {
            var rules = GetPuzzleInput();
            var childLookup = GetChildColours(rules);
            var requiredNumberOfBagsInside = GetRequiredNumberOfBagsWithin("shiny gold", childLookup);
            return "" + requiredNumberOfBagsInside;
        }

        private Dictionary<string, List<(string childColour, int count)>> GetChildColours(string [] rules)
        {
            var childColoursAndCount = new Dictionary<string, List<(string childColour, int count)>>();
            foreach (var rule in rules)
            {
                var bagRegex = new Regex("^(.*) bags contain (.*)$").Match(rule);
                var bagColour = bagRegex.Groups[1].Value;
                var children = new List<(string childColour, int count)>();
                childColoursAndCount.Add(bagColour, children);
                if (rule.Contains("contain no other bags"))
                    continue;

                var subBags = bagRegex.Groups[2].Value.Split(new []{", "}, StringSplitOptions.None);
                foreach (var subBag in subBags)
                {
                    var subBagRegex = new Regex("^(\\d*) (.*) bag").Match(subBag);
                    var count = int.Parse(subBagRegex.Groups[1].Value);
                    var childColour = subBagRegex.Groups[2].Value;
                    children.Add((childColour, count));
                }
            }
            return childColoursAndCount;
        }

        private int GetRequiredNumberOfBagsWithin(string colour, Dictionary<string, List<(string childColour, int count)>> childrenLookup)
        {
            var children = childrenLookup[colour];
            var bagCount = 0;
            foreach (var child in children)
            {
                bagCount += child.count;
                var subBags = GetRequiredNumberOfBagsWithin(child.childColour, childrenLookup);
                bagCount += subBags * child.count;
            }
            return bagCount;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day04 : Day
    {
        public override int DayNumber => 4;

        public override string SolvePartOne()
        {
            var lines = GetPuzzleInput();
            var passportBatch = ParsePassportData(lines);
            var validCount = 0;
            foreach (var passport in passportBatch)
            {
                if (IsValidPassportCidOptional(passport))
                    validCount++;
            }
            return "" + validCount;
        }

        private bool IsValidPassportCidOptional(Dictionary<string, string> passport)
        {
            var required = new List<string>() {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            var result = passport.Keys.Intersect(required).ToList();
            return result.Count == 7;
        }

        List<Dictionary<string, string>> ParsePassportData(string[] batchData)
        {
            var result = new List<Dictionary<string, string>>();


            Dictionary<string, string> passport = new Dictionary<string, string>();
            result.Add(passport);
            foreach (var line in batchData)
            {
                if (line == "")
                {
                    passport = new Dictionary<string, string>();
                    result.Add(passport);
                    continue;
                }

                var bits = line.Split(" ".ToCharArray());
                foreach (var bit in bits)
                {
                    var part = bit.Split(":".ToCharArray());
                    passport.Add(part[0], part[1]);
                }
            }

            return result;
        }

        public override string SolvePartTwo()
        {
            var lines = GetPuzzleInput();
            var passportBatch = ParsePassportData(lines);
            var validCount = 0;
            foreach (var passport in passportBatch)
            {
                if (!IsValidPassportCidOptional(passport))
                    continue;
                
                if (AreRequiredFieldsValid(passport))
                    validCount++;
            }
            return "" + validCount;
        }
        
        private bool AreRequiredFieldsValid(Dictionary<string, string> passport)
        {
            //byr (Birth Year) - four digits; at least 1920 and at most 2002.
            //iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            //eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            //hgt (Height) - a number followed by either cm or in:
            //If cm, the number must be at least 150 and at most 193.
            //If in, the number must be at least 59 and at most 76.
            //hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            //ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            //pid (Passport ID) - a nine-digit number, including leading zeroes.
            //cid (Country ID) - ignored, missing or not.

            var byr = int.Parse(passport["byr"]);
            if (byr < 1920 || byr > 2002)
                return false;

            var iyr = int.Parse(passport["iyr"]);
            if (iyr < 2010 || iyr > 2020)
                return false;

            var eyr = int.Parse(passport["eyr"]);
            if (eyr < 2020 || eyr > 2030)
                return false;

            var hgt = new Regex("^(\\d*)(cm|in)$").Match(passport["hgt"]);
            if (!hgt.Success)
                return false;
            var hgtNumber = int.Parse(hgt.Groups[1].Value);
            if (hgt.Groups[2].Value == "cm")
            {
                if (hgtNumber < 150 || hgtNumber > 193)
                    return false;
            }
            else
            {
                if (hgtNumber < 59 || hgtNumber > 76)
                    return false;
            }

            if (new Regex("^#([0-9a-f]{6})$").Match(passport["hcl"]).Success == false)
                return false;

            if (new Regex("^(amb|blu|brn|gry|grn|hzl|oth)$").Match(passport["ecl"]).Success == false)
                return false;

            if (new Regex("^\\d{9}$").Match(passport["pid"]).Success == false)
                return false;

            return true;
        }
        
    }
}
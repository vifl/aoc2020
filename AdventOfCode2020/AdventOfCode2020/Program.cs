﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;

namespace AdventOfCode2020
{
    internal class Program
    {
        private static int? m_forceDay = null;

        public static void Main(string[] args)
        {
            //m_forceDay = 9;
            
            var created = CreateTodaysClassIfNeeded();
            if (created)
            {
                return;
            }

            var className = GetClassName();
            var typeName = "AdventOfCode2020." + className;
            
            var day = (Day) Activator.CreateInstance("AdventOfCode2020", typeName).Unwrap();
            if (day == null)
            {
                Console.WriteLine("No implementation available for today");
                return;
            }

            var stopwatch = new Stopwatch();
            
            Console.Write($"Solving day {day.DayNumber} part one... ");
            stopwatch.Start();
            var solutionPartOne = day.SolvePartOne();
            stopwatch.Stop();
            Console.WriteLine($"done: {solutionPartOne} (took {stopwatch.Elapsed})");
            
            Console.Write($"Solving day {day.DayNumber} part two... ");
            stopwatch.Restart();
            var solutionPartTwo = day.SolvePartTwo();
            stopwatch.Stop();
            Console.WriteLine($"done: {solutionPartTwo} (took {stopwatch.Elapsed})");
        }

        private static string GetClassName()
        {
            return $"Day{(m_forceDay ?? DateTime.Now.Day):00}";
        }

        private static bool CreateTodaysClassIfNeeded()
        {
            var day = m_forceDay ?? DateTime.Now.Day;
            var year = DateTime.Now.Year;
            var className = GetClassName();
            var projectDirectory = GetBaseDirectory();
            var classFilename = $"{className}.cs";
            var classFileFullPath = Path.Combine(projectDirectory, classFilename);
            var puzzleFilename = $"input{day:00}.txt";
            var puzzleFileFullPath = Path.Combine(projectDirectory, puzzleFilename);

            if (File.Exists(classFileFullPath))
            {
                return false;
            }

            Console.WriteLine($"{classFilename} not found, will create.");
            var classContempt = DayTemplate.Replace("#day#", $"{day:00}");
            File.WriteAllText(classFileFullPath, classContempt);

            Console.Write($"Fetching puzzle input...");
            try
            {
                var puzzleContent = GetPuzzleInput(year, day);
                Console.WriteLine($" done.");
                File.WriteAllText(puzzleFileFullPath, puzzleContent.Trim());
                Console.WriteLine($"Created {puzzleFilename}");
            }
            catch (Exception e)
            {
                Console.WriteLine($" failed: {e.Message}");
                Console.WriteLine($"Created empty {puzzleFilename}");
                File.WriteAllText(puzzleFileFullPath, "");
            }

            var projectFile = Path.Combine(projectDirectory, "AdventOfCode2020.csproj"); 
            
            var p = new Microsoft.Build.Evaluation.Project(projectFile);
            p.AddItem("Compile", classFilename);
            var kvp = new KeyValuePair<string, string>("CopyToOutputDirectory", "PreserveNewest");
            
            p.AddItem("Content", puzzleFilename, new [] {kvp});
            p.Save();

            Console.WriteLine($"Class and data files created. Execution halted. Please edit {classFilename} and then run again.");

            return true;
        }
        
        private static string GetBaseDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            codeBase = codeBase.Replace("file:///", "");

            var dir = Path.GetDirectoryName(codeBase);
            dir = Path.Combine(dir, @"..\..\");
            return Path.GetFullPath(dir);
        }

        private static string GetPuzzleInput(int year, int day)
        {
            using var client = new WebClient();
            var sessionCookie = Environment.GetEnvironmentVariable("session");
            client.Headers.Add(HttpRequestHeader.Cookie, "session=" + sessionCookie);
            var address = $"https://adventofcode.com/{year}/day/{day}/input";
            return client.DownloadString(address);
        }
        
        private const string DayTemplate = @"using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day#day# : Day
    {
        public override string SolvePartOne()
        {
            return null;
        }

        public override string SolvePartTwo()
        {
            return null;
        }
    }
}
"; 
    }
}
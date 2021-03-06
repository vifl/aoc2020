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
        private static readonly int? ForceDay = null; // 9

        public static void Main(string[] args)
        {
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
            return $"Day{(ForceDay ?? DateTime.Now.Day):00}";
        }

        private static bool CreateTodaysClassIfNeeded()
        {
            var day = ForceDay ?? DateTime.Now.Day;
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
            AddToGit(projectDirectory, classFilename);
            Console.WriteLine($"{classFilename} created and added to git");

            Console.Write($"Fetching puzzle input...");
            try
            {
                var puzzleContent = GetPuzzleInput(year, day);
                Console.WriteLine($" done.");
                File.WriteAllText(puzzleFileFullPath, puzzleContent.Trim());
                AddToGit(projectDirectory, puzzleFilename);
                Console.WriteLine($"{puzzleFilename} created and added to git");
            }
            catch (Exception e)
            {
                Console.WriteLine($" failed: {e.Message}");
                File.WriteAllText(puzzleFileFullPath, "");
                AddToGit(projectDirectory, puzzleFilename);
                Console.WriteLine($"Created empty {puzzleFilename} and added to git");
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

        private static void AddToGit(string workingDirectory, string filename)
        {
            var startInfo = new ProcessStartInfo("git", $"add {filename}")
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
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
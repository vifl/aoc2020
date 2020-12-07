using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace AdventOfCode2020
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var className = CreateTodaysClassIfNeeded();
            var typeName = "AdventOfCode2020." + className;
            
            var day = (Day) Activator.CreateInstance("AdventOfCode2020", typeName).Unwrap();
            if (day == null)
            {
                Console.WriteLine("No implementation available for today");
                return;
            }

            Console.WriteLine($"Day {day.DayNumber} part one: {day.SolvePartOne()}");
            Console.WriteLine($"Day {day.DayNumber} part two: {day.SolvePartTwo()}");
        }

        private static string CreateTodaysClassIfNeeded()
        {
            var day = DateTime.Now.Day;
            var year = DateTime.Now.Year;
            var className = $"Day{day:00}";
            var projectDirectory = GetBaseDirectory();
            var classFilename = $"y{className}.cs";
            var classFileFullPath = Path.Combine(projectDirectory, classFilename);
            var puzzleFilename = $"yinput{day:00}.txt";
            var puzzleFileFullPath = Path.Combine(projectDirectory, puzzleFilename);

            if (!File.Exists(classFileFullPath))
            {
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
            }
           
            return className;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day08 : Day
    {
        public override string SolvePartOne()
        {
            var program = GetProgram();
            var ax = Run(program);
            return "" + ax;
        }

        private List<(string operation, int value)> GetProgram()
        {
            var lines = GetPuzzleInput();
            return lines.Select(line => line.Split(" ".ToCharArray())).Select(row => (row[0], int.Parse(row[1]))).ToList();
        }

        private static int Run(List<(string operation, int value)> program)
        {
            var executed = new HashSet<int>();
            var ax = 0;
            var ip = 0;
            do
            {
                executed.Add(ip);
                var (operation, value) = program[ip];
                switch (operation)
                {
                    case "nop":
                        ip++;
                        break;
                    case "acc":
                        ax += value;
                        ip++;
                        break;
                    case "jmp":
                        ip += value;
                        break;
                }
            } while (!executed.Contains(ip));

            return ax;
        }
        
        public override string SolvePartTwo()
        {
            var program = GetProgram();

            for (var rowToModify = 0; rowToModify < program.Count; rowToModify++)
            {
                var (operation, value) = program[rowToModify];
                if (operation == "acc")
                        continue;
                program[rowToModify] = (operation == "nop" ? "jmp" : "nop", value);
                if (TerminatesProperly(program, out var ax))
                    return "" + ax;
                program[rowToModify] = (operation, value);

            }
            return "error";
        }

        private static bool TerminatesProperly(List<(string operation, int value)> program, out int ax)
        {
            var executed = new HashSet<int>();
            ax = 0;
            var ip = 0;
            do
            {
                executed.Add(ip);
                var (operation, value) = program[ip];
                switch (operation)
                {
                    case "nop":
                        ip++;
                        break;
                    case "acc":
                        ax += value;
                        ip++;
                        break;
                    case "jmp":
                        ip += value;
                        break;
                }

                if (ip == program.Count)
                {
                    return true; // terminated properly
                }
            } while (!executed.Contains(ip));
            return false; // infinite loop detected
        }
    }
}
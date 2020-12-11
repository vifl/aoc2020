using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day11 : Day
    {
        public override string SolvePartOne()
        {
            var map = GetMap();
            var iteration = 0;
            while (true)
            {
                var newMap = ApplyRules(map, CalcSeatState);
                //DebugWrite(newMap, iteration);
                if (MapsLooksTheSame(map, newMap) || iteration++ > 100000)
                    break;
                map = newMap;
            }
            return "" +  CountOccupiedSeats(map);
        }

//        private string testData = @"L.LL.LL.LL
//LLLLLLL.LL
//L.L.L..L..
//LLLL.LL.LL
//L.LL.LL.LL
//L.LLLLL.LL
//..L.L.....
//LLLLLLLLLL
//L.LLLLLL.L
//L.LLLLL.LL";

        // null=no seat, false=not occupied, true=occupied
        private bool?[][] GetMap()
        {
            //var lines = testData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var lines = GetPuzzleInput();
            int height = lines.Length + 2;
            int width = lines[0].Length + 2;
            var map = new bool?[height][];
            int rowIndex = 0;
            map[rowIndex++] = Enumerable.Repeat((bool?) null, width).ToArray();
            foreach (var line in lines)
            {
                var row = line.Select(c => c == 'L' ? false : (bool?) null).ToList();
                row.Insert(0, null);
                row.Add(null);
                map[rowIndex++] = row.ToArray();
            }
            map[rowIndex] = Enumerable.Repeat((bool?) null, width).ToArray();
            return map;
        }

        private bool?[][] ApplyRules(bool?[][] map, Func<bool?[][], int, int, bool?> seatStateCalculator)
        {
            var rows = map.Length;
            var seats = map[0].Length;
            var newMap = new bool?[rows][];
            newMap[0] = Enumerable.Repeat((bool?) null, seats).ToArray();
            for (var row = 1; row < map.Length - 1; row++)
            {
                newMap[row] = new bool?[seats];
                newMap[row][0] = null;
                for (var seat = 1; seat < map[row].Length - 1; seat++)
                {
                    newMap[row][seat] = seatStateCalculator.Invoke(map, row, seat);
                }
                newMap[row][seats - 1] = null;
            }
            newMap[rows - 1] = Enumerable.Repeat((bool?) null, seats).ToArray();
            return newMap;
        }

        private bool? CalcSeatState(bool?[][] map, int row, int seat)
        {
            var currentState = map[row][seat];
            if (currentState == null)
                return null;
            var occupiedAdjacentSeats = CountOccupiedAdjacentSeats(map, row, seat);

            if (currentState == false && occupiedAdjacentSeats == 0)
                return true;
            if (currentState == true && occupiedAdjacentSeats >= 4)
                return false;
           
            return currentState;
        }

        private int CountOccupiedAdjacentSeats(bool?[][] map, int row, int seat)
        {
            var count = 0;
            for (var r = row - 1; r <= row + 1; r++)
            {
                for (var s = seat - 1; s <= seat + 1; s++)
                {
                    if (r == row && s == seat)
                        continue;
                    if (map[r][s].HasValue && map[r][s].Value)
                        count++;
                }
            }
            return count;
        }

        private void DebugWrite(bool?[][] newMap, int iteration)
        {
            Console.WriteLine("");
            Console.WriteLine("iteration " + iteration);
            foreach (var row in newMap)
            {
                Console.WriteLine(string.Join("", row.Select(s => s.HasValue ? (s == true ? '#' : 'L') : '.')));
            }
        }

        private bool MapsLooksTheSame(bool?[][] map1, bool?[][] map2)
        {
            if (map1 == null || map2 == null)
                return false;
            if (map1.Length != map2.Length)
                return false;
            for (var i = 0; i < map1.Length; i++)
            {
                if (map1[i].Length != map2[i].Length)
                    return false;
                for (var j = 0; j < map1[i].Length; j++)
                {
                    if (map1[i][j] != map2[i][j])
                        return false;
                }
            }
            return true;
        }

        private int CountOccupiedSeats(bool?[][] map)
        {
            return map.Sum(r => r.Count(s => s.HasValue && s == true));
        }

        public override string SolvePartTwo()
        {
            var map = GetMap();
            var iteration = 0;
            while (true)
            {
                //DebugWrite(map, iteration);
                var newMap = ApplyRules(map, CalcSeatStateVisibleSeats);
                if (MapsLooksTheSame(map, newMap) || iteration++ > 100000)
                    break;
                map = newMap;
            }
            return "" +  CountOccupiedSeats(map);
        }
       
        private bool? CalcSeatStateVisibleSeats(bool?[][] map, int row, int seat)
        {
            var currentState = map[row][seat];
            if (currentState == null)
                return null;
            var occupiedAdjacentSeats = CountOccupiedVisibleSeats(map, row, seat);

            if (currentState == false && occupiedAdjacentSeats == 0)
                return true;
            if (currentState == true && occupiedAdjacentSeats >= 5)
                return false;
           
            return currentState;
        }
       
        private int CountOccupiedVisibleSeats(bool?[][] map, int row, int seat)
        {
            var results = new List<bool?>();
            for (var rowStep = -1; rowStep <= 1; rowStep++)
            {
                for (var seatStep = -1; seatStep <= 1; seatStep++)
                {
                    if (rowStep == 0 && seatStep == 0)
                        continue;

                    results.Add(FirstVisibleSeat(map, row, seat, rowStep, seatStep));
                }
            }
            return results.Count(s => s == true);
        }

        private bool? FirstVisibleSeat(bool?[][] map, int row, int seat, int rowStep, int seatStep)
        {
            var width = map[0].Length; // let's assume the arrangement is always rectangular

            var r = row;
            var s = seat;
            while (true)
            {
                r += rowStep;
                s += seatStep;
                if (r < 0 || r >= map.Length || s < 0 || s >= width)
                    return null;
                
                if (map[r][s].HasValue)
                    return map[r][s].Value;
            }
        }
    }
}
namespace AdventOfCode2020
{
    public class Day03 : Day
    {
        public override int DayNumber => 3;

        public override string SolvePartOne()
        {
            var map = GetPuzzleInput();
            
            var x = 0;
            var y = 0;
            var treeCount = 0;
            while(true)
            {
                x += 3;
                y += 1;
                if (y >= map.Length)
                    break;
                if (IsTree(map, x, y))
                    treeCount++;
            }
            return $"{treeCount}";
        }
        
        private bool IsTree(string[] map, int x, int y)
        {
            var virtualX = x % map[y].Length;
            return map[y][virtualX] == '#';
        }

        public override string SolvePartTwo()
        {
            var map = GetPuzzleInput();

            return "" +
                   GetTreeCount(map, 1, 1) *
                   GetTreeCount(map, 3, 1) *
                   GetTreeCount(map, 5, 1) *
                   GetTreeCount(map, 7, 1) *
                   GetTreeCount(map, 1, 2);

        }

        private long GetTreeCount(string[] map, int stepX, int stepY)
        {
            var x = 0;
            var y = 0;
            var treeCount = 0;
            while(true)
            {
                x += stepX;
                y += stepY;
                if (y >= map.Length)
                    break;
                if (IsTree(map, x, y))
                    treeCount++;
            }

            return treeCount;
        }
    }
}
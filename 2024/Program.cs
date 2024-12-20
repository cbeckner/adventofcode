using System.Data;
using System.Text.RegularExpressions;

Day1();
Day2Alt();
Day3();
Day4();
Day5();
Day6();

#region Day 1
static void Day1()
{
    // load content/day1/input.txt into a string array
    string[] lines = File.ReadAllLines("content/day1/input.txt");
    var list1 = new List<int>();
    var list2 = new List<int>();

    // convert string array to int array
    foreach (var line in lines)
    {
        var elements = line.Split("   ");
        list1.Add(int.Parse(elements[0]));
        list2.Add(int.Parse(elements[1]));
    }

    int similarity = 0;
    foreach (var line in list1)
    {
        similarity += (line * list2.Count(x => x == line));
    }

    list1.Sort();
    list2.Sort();
    int distance = 0;
    for (int i = 0; i < list1.Count; i++)
    {
        distance += Math.Abs(list1[i] - list2[i]);
    }
    Console.WriteLine($"Day 1 = Distance:{distance}, Similarity: {similarity}");
}
#endregion

#region Day 2
static void Day2Alt()
{
    // load content/day2/sample.txt into a string array
    string[] lines = File.ReadAllLines("content/day2/input.txt");

    int safeReports = 0;
    int safeReportsWithDampener = 0;

    foreach (var line in lines)
    {
        var arr = line.Split(' ').Select(int.Parse).ToList();

        // First, check if the report is already safe
        if (Day2IsSafe(arr))
        {
            safeReports++;
            safeReportsWithDampener++; // If it's already safe, it's also safe with dampener
        }
        else
        {
            // If not safe, try removing each level once and see if it becomes safe
            bool dampenerSafe = false;
            for (int i = 0; i < arr.Count; i++)
            {
                var modifiedArr = new List<int>(arr);
                modifiedArr.RemoveAt(i);

                if (Day2IsSafe(modifiedArr))
                {
                    dampenerSafe = true;
                    break;
                }
            }

            if (dampenerSafe)
            {
                // Not counted as safe in the original scenario
                // but is safe with dampener
                safeReportsWithDampener++;
            }
        }
    }

    Console.WriteLine($"Day 2 = Safe Reports: {safeReports}, Safe Reports (with Dampener): {safeReportsWithDampener}");
}

static bool Day2IsSafe(List<int> arr)
{
    if (arr.Count < 2)
    {
        // If there's only one level or none, we can consider it trivially safe
        return true;
    }

    // Calculate differences
    List<int> diffs = new List<int>();
    for (int i = 1; i < arr.Count; i++)
    {
        diffs.Add(arr[i] - arr[i - 1]);
    }

    // No zero differences allowed
    if (diffs.Any(d => d == 0))
        return false;

    // Differences must be between -3 and -1 (for decreasing)
    // or between 1 and 3 (for increasing)
    if (diffs.Any(d => Math.Abs(d) > 3))
        return false;

    // All diffs must be strictly positive or all strictly negative
    bool allPositive = diffs.All(d => d > 0);
    bool allNegative = diffs.All(d => d < 0);

    return allPositive || allNegative;
}
#endregion

#region Day 3
static void Day3()
{
    // load content/day2/sample.txt into a string array
    string[] lines = File.ReadAllLines("content/day3/input.txt");
    var input = string.Join("", lines);
    var instructions = new List<Tuple<int, int>>();
    var result = 0;
    var filteredResult = 0;
    var regex = new Regex(@"mul\(([+-]?\d+),([+-]?\d+)\)");
    var filterRegex = new Regex(@"don't\(\)(.*?)do\(\)");

    var matches = regex.Matches(input);
    foreach (Match match in matches)
    {
        var x = Convert.ToInt32(match.Groups[1].Value);
        var y = Convert.ToInt32(match.Groups[2].Value);
        instructions.Add(new Tuple<int, int>(x, y));
        result += x * y;
    }

    var newInput = input;
    while (newInput.Contains("don't()", StringComparison.CurrentCulture))
    {
        var subLine = newInput[newInput.IndexOf("don't()")..];
        var end = subLine.IndexOf("do()");
        subLine = subLine[..(end + 3)];
        newInput = newInput.Replace(subLine, "");
    }

    var newMatches = regex.Matches(newInput);
    foreach (Match match in newMatches)
    {
        var x = Convert.ToInt32(match.Groups[1].Value);
        var y = Convert.ToInt32(match.Groups[2].Value);
        instructions.Add(new Tuple<int, int>(x, y));
        filteredResult += x * y;
    }

    Console.WriteLine($"Day 3 = Total: {result}.  Filtered Total: {filteredResult}");
}
#endregion

#region Day 4
static void Day4()
{
    // load content/day4/input.txt into a string array
    string[] lines = File.ReadAllLines("content/day4/input.txt");
    char[,] grid = new char[lines.Length, lines[0].Length];

    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            grid[i, j] = lines[i][j];
        }
    }

    int xmasCount = 0;
    int xMasCount = 0;
    for (int i = 0; i < grid.GetLength(0); i++)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            // Check vertically down (XMAS)
            if ((i + 3) < grid.GetLength(0)
                && grid[i, j] == 'X'
                && grid[i + 1, j] == 'M'
                && grid[i + 2, j] == 'A'
                && grid[i + 3, j] == 'S')
            {
                xmasCount++;
            }

            // Check vertically down (SAMX)
            if ((i + 3) < grid.GetLength(0)
                && grid[i, j] == 'S'
                && grid[i + 1, j] == 'A'
                && grid[i + 2, j] == 'M'
                && grid[i + 3, j] == 'X')
            {
                xmasCount++;
            }

            // Check horizontally to the right (XMAS)
            if ((j + 3) < grid.GetLength(1)
                && grid[i, j] == 'X'
                && grid[i, j + 1] == 'M'
                && grid[i, j + 2] == 'A'
                && grid[i, j + 3] == 'S')
            {
                xmasCount++;
            }

            // Check horizontally to the right (SAMX)
            if ((j + 3) < grid.GetLength(1)
                && grid[i, j] == 'S'
                && grid[i, j + 1] == 'A'
                && grid[i, j + 2] == 'M'
                && grid[i, j + 3] == 'X')
            {
                xmasCount++;
            }

            // Check diagonally down-right (XMAS)
            if ((i + 3) < grid.GetLength(0) && (j + 3) < grid.GetLength(1)
                && grid[i, j] == 'X'
                && grid[i + 1, j + 1] == 'M'
                && grid[i + 2, j + 2] == 'A'
                && grid[i + 3, j + 3] == 'S')
            {
                xmasCount++;
            }

            // Check diagonally down-right (SAMX)
            if ((i + 3) < grid.GetLength(0) && (j + 3) < grid.GetLength(1)
                && grid[i, j] == 'S'
                && grid[i + 1, j + 1] == 'A'
                && grid[i + 2, j + 2] == 'M'
                && grid[i + 3, j + 3] == 'X')
            {
                xmasCount++;
            }

            // Check diagonally down-left (XMAS)
            if ((i + 3) < grid.GetLength(0) && (j - 3) >= 0
                && grid[i, j] == 'X'
                && grid[i + 1, j - 1] == 'M'
                && grid[i + 2, j - 2] == 'A'
                && grid[i + 3, j - 3] == 'S')
            {
                xmasCount++;
            }

            // Check diagonally down-left (SAMX)
            if ((i + 3) < grid.GetLength(0) && (j - 3) >= 0
                && grid[i, j] == 'S'
                && grid[i + 1, j - 1] == 'A'
                && grid[i + 2, j - 2] == 'M'
                && grid[i + 3, j - 3] == 'X')
            {
                xmasCount++;
            }

        }
    }

    for (int i = 1; i < grid.GetLength(0) - 1; i++)
    {
        for (int j = 1; j < grid.GetLength(1) - 1; j++)
        {
            if (grid[i, j] == 'A')
            {
                if (((grid[i - 1, j - 1] == 'M' && grid[i + 1, j + 1] == 'S')
                    || (grid[i - 1, j - 1] == 'S' && grid[i + 1, j + 1] == 'M')) &&
                    ((grid[i - 1, j + 1] == 'M' && grid[i + 1, j - 1] == 'S') ||
                    (grid[i - 1, j + 1] == 'S' && grid[i + 1, j - 1] == 'M')))
                {
                    xMasCount++;
                }
            }
        }
    }
    Console.WriteLine($"Day 4 = Total: {xmasCount}. X-Mas Total: {xMasCount}");
}
#endregion

#region Day 5
static void Day5()
{
    string[] lines = File.ReadAllLines("content/day5/input.txt");
    var rules = new List<KeyValuePair<int, int>>();
    var updates = new List<int[]>();

    foreach (string line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            continue;
        }
        if (line.Contains('|'))
        {
            var parts = line.Split('|');
            rules.Add(new KeyValuePair<int, int>(int.Parse(parts[0]), int.Parse(parts[1])));
        }
        else
        {
            updates.Add(line.Split(',').Select(int.Parse).ToArray());
        }
    }

    var ruleDict = new Dictionary<int, int[]>();
    foreach (var rule in rules)
    {
        if (ruleDict.ContainsKey(rule.Key)) { continue; }
        ruleDict.Add(rule.Key, rules.Where(x => x.Key == rule.Key).Select(x => x.Value).ToArray());
    }

    var safeUpdates = new List<int[]>();
    var unsafeUpdates = new List<int[]>();
    foreach (var update in updates)
    {
        bool safe = true;
        foreach (var page in update)
        {
            if (!ruleDict.ContainsKey(page))
            {
                continue;
            }
            var pages = ruleDict[page];
            foreach (var p in pages)
            {
                if (!update.Contains(p))
                {
                    continue;
                }
                var index1 = Array.IndexOf(update, page);
                var index2 = Array.IndexOf(update, p);
                if (index1 > index2)
                {
                    safe = false;
                    break;
                }
            }
        }
        if (safe)
        {
            safeUpdates.Add(update);
        }
        else
        {
            unsafeUpdates.Add(update);
        }
    }
    // Get the middle value of each safe update and add it.
    var correctTotal = 0;
    foreach (var update in safeUpdates)
    {
        var middle = Convert.ToInt32(Math.Floor(update.Length / 2M));
        correctTotal += update[middle];
    }

    // For each of the incorrectly-ordered updates, use the page ordering rules to put the page numbers in the right order.
    var newSafeUpdates = new List<int[]>();
    while (unsafeUpdates.Count > 0)
    {
        for (int i = 0; i < unsafeUpdates.Count; i++)
        {
            var update = unsafeUpdates[i];
            bool safe = true;
            foreach (var page in update)
            {
                if (!ruleDict.ContainsKey(page))
                {
                    continue;
                }
                var pages = ruleDict[page];
                foreach (var p in pages)
                {
                    if (!update.Contains(p))
                    {
                        continue;
                    }
                    var index1 = Array.IndexOf(update, page);
                    var index2 = Array.IndexOf(update, p);
                    if (index1 > index2)
                    {
                        update = SwapValues<int>(update, index1, index2);
                        safe = false;
                        break;
                    }
                }
            }
            if (safe)
            {
                newSafeUpdates.Add(update);
                unsafeUpdates.RemoveAt(i);
                i--; // Adjust index after removal
            }
            else
            {
                unsafeUpdates[i] = update; // Update the modified array back to the list
            }
        }
    }
    // Get the middle value of each new safe update and add it.
    var newCorrectTotal = 0;
    foreach (var update in newSafeUpdates)
    {
        var middle = Convert.ToInt32(Math.Floor(update.Length / 2M));
        newCorrectTotal += update[middle];
    }

    Console.WriteLine($"Day 5 = Safe Updates: {correctTotal}. New Safe Updates: {newCorrectTotal}");
}

static T[] SwapValues<T>(T[] source, int index1, int index2)
{
    T temp = source[index1];
    source[index1] = source[index2];
    source[index2] = temp;
    return source;
}
#endregion

#region Day 6
static void Day6()
{
    string[] lines = File.ReadAllLines("content/day6/input.txt");
    var maxX = lines.Length;
    var maxY = lines[0].Length;
    var map = new string[maxX, maxY];

    var guard = new Guard() { X = 0, Y = 0, Direction = "^" };

    // Read the lines into an array
    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            map[i, j] = lines[i][j].ToString();
            // Find the starting point
            if (map[i, j] == "^" || map[i, j] == ">" || map[i, j] == "<" || map[i, j] == "v")
            {
                guard = new Guard() { X = i, Y = j, Direction = map[i, j] };
            }
        }
    }

    // Parse the map
    while (guard.X < maxX && guard.X >= 0 && guard.Y < maxY && guard.Y >= 0)
    {
        guard.Move(ref map);
    }
    // Count the X's in the map
    int count = 0;
    for (int i = 0; i < maxX; i++)
    {
        //Console.WriteLine();
        for (int j = 0; j < maxY; j++)
        {
            //Console.Write(map[i, j]);
            if (map[i, j] == "X")
            {
                count++;
            }
        }
    }
    Console.WriteLine($"Day 6 = Count: {count}");
}

struct Guard()
{
    public int X { get; set; }
    public int Y { get; set; }
    public required string Direction { get; set; }

    public void MoveNorth()
    {
        X--;
        Direction = "^";
    }
    public void MoveSouth()
    {
        X++;
        Direction = "v";
    }
    public void MoveEast()
    {
        Y++;
        Direction = ">";
    }
    public void MoveWest()
    {
        Y--;
        Direction = "<";
    }
    public void TurnRight()
    {
        switch (Direction)
        {
            case "^":
                Direction = ">";
                break;
            case ">":
                Direction = "v";
                break;
            case "v":
                Direction = "<";
                break;
            case "<":
                Direction = "^";
                break;
        }
    }

    public void Move(ref string[,] map)
    {
        if (map == null) { return; }
        if (map.GetLength(0) < 0 || map.GetLength(1) < 0) { return; }
        if (X > map.GetLength(0) || Y > map.GetLength(1) || X < 0 || Y < 0)
        {
            return;
        }
        map[X, Y] = "X";
        switch (Direction)
        {
            case "^":
                if (X - 1 < 0)
                {
                    MoveNorth();
                    return;
                }
                if (map[X - 1, Y] == "#")
                {
                    TurnRight();
                    return;
                }
                MoveNorth();
                break;
            case ">":
                if (Y + 1 >= map.GetLength(1))
                {
                    MoveEast();
                    return;
                }
                if (map[X, Y + 1] == "#")
                {
                    TurnRight();
                    return;
                }
                MoveEast();
                break;
            case "v":
                if (X + 1 >= map.GetLength(0))
                {
                    MoveSouth();
                    return;
                }
                if (map[X + 1, Y] == "#")
                {
                    TurnRight();
                    return;
                }
                MoveSouth();
                break;
            case "<":
                if (Y - 1 < 0)
                {
                    MoveWest();
                    return;
                }
                if (map[X, Y - 1] == "#")
                {
                    TurnRight();
                    return;
                }
                MoveWest();
                break;
        }
    }
}
#endregion
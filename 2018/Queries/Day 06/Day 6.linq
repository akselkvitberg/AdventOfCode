<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    var result1 = Solve1(data).Dump("1");
	var result2 = Solve2(data).Dump("2");
}

private int Solve1(IEnumerable<Point> data)
{
    var points = data.ToList();
    
    var maxX = points.Max(p=>p.X);
    var minX = points.Min(p=>p.X);
    var maxY = points.Max(p=>p.Y);
    var minY = points.Min(p=>p.Y);
    
    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            var ps = points.GroupBy(p => p.GetManhattanDistance(x, y)).OrderBy(g => g.Key).First();
            if (ps.Count() == 1)
            {
                var point = ps.Single();
                point.OwnedPoints++;
                if (x == minX || x == maxX || y == minY || y == maxY)
                {
                    point.IsInfinite = true;
                }
            }
        }
    }

    var pointWithMostSafePoints = points.Where(x=>x.IsInfinite == false).OrderByDescending(x=>x.OwnedPoints).First();
    
    
	return pointWithMostSafePoints.OwnedPoints;
}

private int Solve2(IEnumerable<Point> data)
{
    var points = data.ToList();

    var maxX = points.Max(p => p.X);
    var minX = points.Min(p => p.X);
    var maxY = points.Max(p => p.Y);
    var minY = points.Min(p => p.Y);
    
    int safePoints = 0;
    
    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            var sum = points.Select(p=>p.GetManhattanDistance(x,y)).Sum();
            if(sum < 10000)
            {
                safePoints++;
            }
        }
    }
    
    return safePoints;
}

private IEnumerable<Point> ProcessInput(string input)
{
    var data = input.SplitLines().Select(x => x.Split(new[] { ", "}, StringSplitOptions.RemoveEmptyEntries)).Select(x=>new Point(int.Parse(x[0]), int.Parse(x[1])));
    return data;
}

class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
    
    public int OwnedPoints { get; set; }
    public bool IsInfinite { get; set; }
    
    public int GetManhattanDistance(int x, int y)
    {
        return Math.Abs(X - x) + Math.Abs(Y - y);
    }
}
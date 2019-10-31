<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    await Solve1(data);
	//var result2 = Solve2(data).Dump("2");
}

private async Task Solve1(IEnumerable<Point> data)
{
    var points = data.ToList();
    
    var lastMin = points.Max(x => x.X) - points.Min(x=>x.X);
    for (int i = 0; i < 10159; i++) // Trial and error
    {
        foreach (var point in points)
        {
            point.MoveNext();
        }
        var currentMin = points.Max(x => x.X) - points.Min(x=>x.X);
        //currentMin.Dump();
        if(currentMin >= lastMin)
            break;
        lastMin = currentMin;
    }

    var minX = points.Min(p => p.X);
    var minY = points.Min(p => p.Y);
    foreach (var p in points)
    {
        p.X -= minX;
        p.Y -= minY;
    }

    var minx = points.Min(p => p.X);
    var miny = points.Min(o => o.Y);
    var maxx = points.Max(x => x.X);
    var maxy = points.Max(y => y.Y);

    int scale = 1;
    
    

    Bitmap b = new Bitmap(maxx / scale, maxy / scale, PixelFormat.Format32bppRgb);
	Graphics g = Graphics.FromImage(b);
	g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
	g.Dispose();

    foreach (var point in points)
    {
        ///point.MoveNext();
        b.SetPixel(Math.Max(Math.Min(point.X / scale, b.Width - 1), 0), Math.Max(Math.Min(point.Y / scale, b.Height - 1), 0), Color.Black);
    }
    b.Dump();
    
    
}

private int Solve2(IEnumerable<Point> data)
{
	return 10159; // trial and error
}

private IEnumerable<Point> ProcessInput(string input)
{
    Regex regex = new Regex(@"position=\<(.*), (.*)\> velocity=\<(.*), (.*)\>");
    var lines = input.SplitLines();
    var points = lines.Select(x=>regex.Match(x)).Select(x=>new Point(x));
    
    points = points.ToList();
    var minX = points.Min(p=>p.X);
    var minY = points.Min(p=>p.Y);
    foreach (var p in points)
    {
        p.X -= minX;
        p.Y -= minY;
    }
    
	return points;
}

class Point
{

    public Point(Match m)
    {
        X = int.Parse(m.Groups[1].Value);
        Y = int.Parse(m.Groups[2].Value);
        dX = int.Parse(m.Groups[3].Value);
        dY = int.Parse(m.Groups[4].Value);
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int dX { get; set; }
    public int dY { get; set; }
    
    public void MoveNext()
    {
        X += dX;
        Y += dY;
    }
}
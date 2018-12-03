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

private int Solve1(IEnumerable<Claim> data)
{
    int[] canvas = new int[1000*1000];
    
    foreach (var claim in data)
    {
        for (int x = claim.X; x < claim.Width + claim.X; x++)
        {
            for (int y = claim.Y; y < claim.Height + claim.Y; y++)
            {
                canvas[(x) + (y * 1000)]++;
            }
        }
    }

    return canvas.Count(x => x > 1);;
}

private int Solve2(IEnumerable<Claim> data)
{
    Dictionary<int, List<Claim>> canvas = new Dictionary<int, List<Claim>>();

    foreach (var claim in data)
    {
        for (int x = claim.X; x < claim.Width + claim.X; x++)
        {
            for (int y = claim.Y; y < claim.Height + claim.Y; y++)
            {
                var pos = (x) + (y * 1000);
                if(!canvas.ContainsKey(pos))
                    canvas[pos] = new List<Claim>();
                var list = canvas[pos];
                list.Add(claim);
                
                if(list.Count > 1)
                    foreach (var overlappingClaim in list)
                    {
                        overlappingClaim.Overlaps = true;
                    }
            }
        }
    }
    
    return data.Single(x=>x.Overlaps == false).Id;
}

private List<Claim> ProcessInput(string input)
{
	return input.SplitLines().Select(x=>new Claim(x)).ToList();
}

public class Claim
{
    static Regex regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");
    public Claim(string line)
    {
       var groups = regex.Match(line).Groups;
       Id = int.Parse(groups[1].Value);
       X = int.Parse(groups[2].Value);
       Y = int.Parse(groups[3].Value);
       Width = int.Parse(groups[4].Value);
       Height = int.Parse(groups[5].Value);
    }
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Overlaps { get; set; }
}
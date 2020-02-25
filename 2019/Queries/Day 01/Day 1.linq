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

int GetFuel(int x) => (int)(x / 3) - 2;
int GetFuel2(int x) 
{
    var fuel = (int)(x / 3) - 2;
    
    if (fuel <= 0)
        return 0;
        
    return GetFuel2(fuel) + fuel;
}

private int Solve1(IEnumerable<int> data)
{
    return data.Sum(GetFuel);
}

private int Solve2(IEnumerable<int> data)
{
    
    // Initial fuel required
    var fuel = data.Sum(GetFuel2);
    
    //List<int> modules = data.Select(GetFuel).Where(x=>x>0).ToList();
    //
    //while (modules.Count != 0)
    //{
    //    fuel += modules.Sum(GetFuel);
    //    modules = modules.Select(GetFuel).Where(x=>x > 0).ToList();
    //}
    
	return fuel;
}

private IEnumerable<int> ProcessInput(string input)
{
	return input.SplitLines().Select(int.Parse).ToList();
}
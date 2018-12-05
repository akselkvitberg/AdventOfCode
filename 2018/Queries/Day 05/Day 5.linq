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

private int Solve1(IEnumerable<char> input)
{
    var data = input.ToList();
	for (int i = 0; i < data.Count-1; i++)
    {
        var diff = Math.Abs(data[i] - data[i+1]);
        if(diff == 32)
        {
            data.RemoveAt(i);
            data.RemoveAt(i);
            i --;
            if(i > -1)
                i--;
        }
    }

    return data.Count;
}

private int Solve2(IEnumerable<char> data)
{
    var min = data.Count();
	for (char c = 'a'; c <= 'z'; c++)
    {
        var list = data.Where(x => x != c && x != char.ToUpper(c)).ToList();
        var result = Solve1(list);
        min = Math.Min(result, min);
    }
    
    return min;
}

private IEnumerable<char> ProcessInput(string input)
{
    return input.Trim().ToCharArray();
}
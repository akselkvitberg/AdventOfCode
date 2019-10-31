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

private int Solve1(IEnumerable<int> data)
{
	throw new NotImplementedException();
}

private int Solve2(IEnumerable<int> data)
{
	throw new NotImplementedException();
}

private IEnumerable<int> ProcessInput(string input)
{
	throw new NotImplementedException();
}

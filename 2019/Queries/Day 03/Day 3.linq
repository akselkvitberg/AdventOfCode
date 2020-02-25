<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    //var input = await AdventOfCode.GetInput();
    var input = "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83";
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
    input.SplitLines().Dump();
	throw new NotImplementedException();
}
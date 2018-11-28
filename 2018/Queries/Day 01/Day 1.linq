<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    var result = Solve(data);
    
    AdventOfCode.Submit(result).Dump("Result");
}

private string Solve(object data)
{
    throw new NotImplementedException();
}

private object ProcessInput(string input)
{
    throw new NotImplementedException();
}
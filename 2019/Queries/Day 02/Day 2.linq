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
    var program = data.ToArray();
    program[1] = 12;
    program[2] = 2;
	int cur = 0;

    while (true)
    {

        var opCode = program[cur];
        if (opCode == 99)
            return program[0];

        var arg1 = program[cur + 1];
        var arg2 = program[cur + 2];
        var arg3 = program[cur + 3];

        var val1 = program[arg1];
        var val2 = program[arg2];

        program[arg3] = opCode switch
        {
            1 => val1 + val2,
            2 => val1 * val2,
            _ => throw new ArgumentException()
        };

        cur += 4;
    }
}

private int Solve2(IEnumerable<int> data)
{
	throw new NotImplementedException();
}

private IEnumerable<int> ProcessInput(string input)
{
    return input.SplitComma().Parse();
}
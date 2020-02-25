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
    
    RunIntcode(program);
    
    return program[0];
    
}

private int Solve2(IEnumerable<int> data)
{
    for (int noun = 0; noun < 100; noun++)
    {
        for (int verb = 0; verb < 100; verb++)
        {
            var program = data.ToArray();

            program[1] = noun;
            program[2] = verb;
            RunIntcode(program);
            if(program[0] == 19690720)
                return 100 * noun + verb;
        }
    }
    throw new Exception();
}

private void RunIntcode(int[] program)
{
    int cur = 0;
    while (true)
    {
        var opCode = program[cur];
        if (opCode == 99)
            return;

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

private IEnumerable<int> ProcessInput(string input)
{
    return input.SplitComma().Parse();
}
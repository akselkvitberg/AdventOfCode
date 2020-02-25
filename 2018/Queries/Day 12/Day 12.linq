<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    var result1 = Solve1(data.initial, data.rules).Dump("1");
	//var result2 = Solve2(data.initial, data.rules).Dump("2");
}

private int Solve1(string initial, IEnumerable<string> rules)
{
    initial.Dump();
    var data = initial;
    
    var zeroCount = -4;

    for (int l = 0; l < 20; l++)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append("000");

        for (int i = 2; i < data.Length - 2; i++)
        {
            var check = data.Substring(i - 2, 5);
            if (rules.Contains(check))
            {
                sb.Append("1");
            }
            else
                sb.Append("0");
        }
        
        sb.Append("000");
        
        data = sb.ToString();
        if(!data.StartsWith("0000"))
        {
            zeroCount -= 4;
            data = "0000" + data;
        }
        data.Dump();
        
    }
    
    zeroCount.Dump();
    
    return data.Count(z => z == '1');
}

private int Solve2(string initial, IEnumerable<string> rules)
{
	throw new NotImplementedException();
}

private (string initial, IEnumerable<string> rules) ProcessInput(string input)
{
    var lines = input.Replace('#', '1').Replace('.', '0').SplitLines().ToList();
    
    var initial = "0000" + lines[0].Substring(15) + "0000";

    var rules = lines.Skip(1).OrderBy(z => z)
    .Select(x => x.Split(" => ")).Select(x => new { mark = x[0], result = x[1]})
    .Where(x=>x.result == "1").Select(x=>x.mark);
    
    return (initial, rules);
}
<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    var result1 = Solve1(data.start, data.end).Dump("1");
	var result2 = Solve2(data.start, data.end).Dump("2");
}

private int Solve1(int start, int end)
{
    int match = 0;
    for (int i = start; i <= end; i++)
    {
        var digits = i.ToString().AsEnumerable().Select(x=>int.Parse(x.ToString())).ToList() ;
        if(VerifyDigits(digits))
            match++;
    }
    return match;
}

bool VerifyDigits(List<int> digits)
{
    var c = digits[0];
    bool sameDigits = false;
    for (int i = 1; i < digits.Count; i++)
    {
        if(digits[i] < c)
            return false;
        if(digits[i] == c)
            sameDigits = true;
        c = digits[i];
    }
    
    return sameDigits;
}

private int Solve2(int start, int end)
{
	throw new NotImplementedException();
}

private (int start, int end) ProcessInput(string input)
{
    var data = input.Split('-').Parse().ToList();
    return(data[0], data[1]);
}
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

private int Solve1(IEnumerable<string> data)
{
	int twice = 0;
	int thrice = 0;
	foreach (var line in data)
	{
		var groups = line.GroupBy(c=>c);
		if(groups.Any(x=>x.Count() == 3))
			thrice++;
		if (groups.Any(x=>x.Count() == 2))
			twice++;
	}
	return twice * thrice;
}

private string Solve2(IEnumerable<string> data)
{
	List<string> boxes = new List<string>();


	//string.Join("", data.SelectMany(y => y.Select((x, i) => new { x, i})).GroupBy(x=>x).Where(x=>x.Count() == 4).Select(x=>x.Key.x)).Dump();

	foreach (var line1 in data)
		foreach (var line2 in data)
		{
			if (line1 == line2)
				continue;

			var diff = 0;
			for (int i = 0; i < line1.Length; i++)
			{
				if (line1[i] != line2[i])
					diff++;
			}
			if (diff <= 1)
			{
				boxes.Add(line1);
			}
		}


	string s = "";
	for (int i = 0; i < boxes[0].Length; i++)
	{
		if(boxes[0][i] == boxes[1][i])
		{
			s+= boxes[0][i];
		}
	}
	
	return s;
	
	//string.Join("", boxes.SelectMany((i,x)=>x);//.GroupBy(x=>x).OrderBy(x=>x.Count()).Where(x=>x.Count() == 4).Select(x=>x.Key));
	
	return "";
}

private IEnumerable<string> ProcessInput(string input)
{
	return input.SplitLines();
}
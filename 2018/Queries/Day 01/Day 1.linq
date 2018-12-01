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
    return data.Sum();
}

private int Solve2(IEnumerable<int> data)
{
	Queue<int> queue = new Queue<int>(data);
	HashSet<int> set = new HashSet<int>();
	
	int count = 0;
	while (true)
	{
		var i = queue.Dequeue();
		queue.Enqueue(i);
		count += i;
		
		if(set.Contains(count))
		{
			return count;
		}
		set.Add(count);
	}
}

private IEnumerable<int> ProcessInput(string input)
{
	return input.SplitLines().Select(x => int.Parse(x));
}
<Query Kind="Program">
  <Connection>
    <ID>c8d60652-c8e4-41d0-9306-7763edb07f9c</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <Database>TransMed8</Database>
  </Connection>
</Query>

void Main()
{
	var input = "10	3	15	10	5	15	5	15	9	2	5	8	5	2	3	6".Split('\t').Select((x, i) => new Block { Index = i, Blocks = int.Parse(x) }).ToList();

	Dictionary<string, int> knownBlocks = new Dictionary<string, int>();
	int counter = 0;
	string combination;
	while (true)
	{
		combination = string.Join("\t", input.Select(x=>string.Format("{0:00}",x.Blocks)));
		if(knownBlocks.ContainsKey(combination))
			break;
		knownBlocks.Add(combination, counter);
		
		counter++;
		var max = input.OrderByDescending(x => x.Blocks).ThenBy(x => x.Index).First();
		var blocks = max.Blocks;
		max.Blocks = 0;
		
		int index = max.Index;
		while(blocks > 0)
		{
			index = (index + 1) % input.Count;
			input[index].Blocks++;
			blocks--;
		}
	}
	
	counter.Dump();
	var indexOfLoopStart = knownBlocks[combination];
	var cycles = counter - indexOfLoopStart;
	cycles.Dump();
	
}

class Block
{
	public int Index { get; set; }
	public int Blocks { get; set; }
}
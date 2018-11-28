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
	
	var input = "206,63,255,131,65,80,238,157,254,24,133,2,16,0,1,3";
	var data = input.Split(',').Select(x=> byte.Parse(x));
	
	var state = Enumerable.Range(0,256).ToList();
	
	var position = 0;
	var skip = 0;
	foreach (var length in data)
	{
		Reverse(state, position, position + length);
		position = (position + length + skip++) % 256;
	}
	
	var res = state[0] * state[1];
	res.Dump();
}

void Reverse(List<int> state, int start, int end)
{
	var data = state.ToList();
	int count = end - start;
	for (int i = 0; i < count; i++)
	{
		var inputPos = (start + i) % 256;
		var srcPos = (end - i - 1) % 256;
		//Console.WriteLine($"{inputPos}, {srcPos}");
		state[inputPos] = data[srcPos];
	}
}
<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Inputs", "day8.txt");
	var lines = File.ReadAllLines(file).ToList();
	
	var registry = new Dictionary<string, int>();
	var max = 0;
	
	foreach (var line in lines)
	{
		var elements = line.Split(' ');
		var reg1 = elements[0];
		int val = int.Parse(elements[2]);
		var diff = elements[1] == "dec" ? -val : val;
		var conditional = elements[5];
		var reg2 = elements[4];
		var a2 = int.Parse(elements[6]);
		
		if(!registry.ContainsKey(reg1))
		{
			registry.Add(reg1, 0);
		}
		
		registry.TryGetValue(reg2, out var reg2Val);
		
		var comparison = Compare(reg2Val, a2, conditional);
		if(comparison)
		{
			registry[reg1] += diff;
			max = Math.Max(registry[reg1], max);
		}
	}
	
	
	registry.Values.Max().Dump();
	max.Dump();
	
	
}

public static bool Compare(int first, int second, string op)
{
	switch (op)
	{
		case ">": return first > second;
		case ">=": return first >= second;
		case "<": return first < second;
		case "<=": return first <= second;
		case "==": return first == second;
		case "!=": return first != second;
	}

	return false;
}

// Define other methods and classes here

<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Inputs", "day9.txt");
	var input = File.ReadAllText(file).ToCharArray();

	bool skip = false;
	bool isGarbage = false;
	int groupSum = 0;
	int groupCount = 0;
	int garbageCount = 0;
	foreach (var c in input)
	{
		if(skip)
		{
			skip = false;
			continue;
		}
		if(c == '!')
		{
			skip = true;
			continue;
		}
		if (isGarbage)
		{
			if (c == '>')
			{
				isGarbage = false;
			}
			else
				garbageCount++;
			continue;
		}
		switch (c)
		{
			case '{':
				groupCount++;
				
				groupSum += groupCount;
				break;
			case '}':
				groupCount--;
				break;
			case '<':
				isGarbage = true;
				break;
		}
	}
	
	groupSum.Dump("Sum");
	garbageCount.Dump("garbage");
}
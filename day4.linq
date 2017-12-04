<Query Kind="Statements">
  <Connection>
    <ID>c8d60652-c8e4-41d0-9306-7763edb07f9c</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <Database>TransMed8</Database>
  </Connection>
</Query>

var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath),"Inputs","day4.txt");
var lines = File.ReadAllLines(file);

var sum = lines.Count(l => l.Split(' ').GroupBy(w => w).All(g=>g.Count() == 1));
sum.Dump("1");

sum = 0;
foreach (var line in lines)
{
	
	var words = line.Split(' ');
	var invalid = false;
	
	var list = new List<List<char>>();
	foreach (var word in words)
	{
		var orderedWord = word.ToCharArray().OrderBy(x=>x).ToList();
		if(list.Any(x=>orderedWord.SequenceEqual(x)))
		{
			invalid = true;
		}
		list.Add(orderedWord);
	}
	if(!invalid)
	{
		sum++;
	}
}
sum.Dump("2");
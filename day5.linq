<Query Kind="Statements">
  <Connection>
    <ID>c8d60652-c8e4-41d0-9306-7763edb07f9c</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <Database>TransMed8</Database>
  </Connection>
</Query>

var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Inputs", "day5.txt");
var lines = File.ReadAllLines(file).Select(x=> int.Parse(x)).ToList();

int index = 0;
int steps = 0;
while(index < lines.Count)
{
	var offset = lines[index]++;
	index += offset;
	steps++;
}
steps.Dump();

lines = File.ReadAllLines(file).Select(x=> int.Parse(x)).ToList();
index = 0;
steps = 0;
while (index < lines.Count)
{
	var offset = lines[index]++;
	if(offset >= 3)
	{
		lines[index]-=2;
	}
	index += offset;
	steps++;
}
steps.Dump();
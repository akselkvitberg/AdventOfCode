<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Inputs", "day7.txt");
	var lines = File.ReadAllLines(file).ToList();

	var regex = new Regex(@"(\w+) \((\d+)\)( -> ([\w, ]+))?");
	
	Dictionary<string, Node> nodes = new Dictionary<string, Node>();
	
	foreach (var line in lines)
	{
		var matches = regex.Match(line);
		var name = matches.Groups[1].Value;
		var weight = int.Parse(matches.Groups[2].Value);
		var node = new Node
		{
			Name = name,
			Weight = weight,
		};
		nodes.Add(name, node);
		node.ChildrenNames = matches.Groups[4].Value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).ToList();
		
	}
	
	foreach (var node in nodes.Values)
	{
		foreach (var childrenName in node.ChildrenNames)
		{
			var child = nodes[childrenName];
			node.Children.Add(child);
			child.Parent = node;
		}
	}
	
	var firstNode = nodes.FirstOrDefault(x=>x.Value.Parent == null).Value;
	firstNode.Name.Dump("Part 1");
	
	Balance(firstNode);	
}

int Balance(Node node)
{
	if(!node.Children.Any())
		return node.Weight;

	var weights = node.Children.Select(x => new { Node = x, TotalWeight = Balance(x)});
	
	var groups = weights.GroupBy(x=>x.TotalWeight);
	if(groups.Count() >= 2)
	{
		var wrongNode = groups.First(x=>x.Count() == 1).First();
		var correctWeight = groups.First(x=>x.Count() != 1).First().TotalWeight;
		
		var diff = wrongNode.TotalWeight - correctWeight;
		wrongNode.Node.Weight -= diff;
		weights = node.Children.Select(x => new { Node = x, TotalWeight = Balance(x)});
		
		Console.WriteLine("Wrong node: " + wrongNode.Node.Name + " with " +diff + " error");
		Console.WriteLine("New weight: " + wrongNode.Node.Weight);
	}
	
	return weights.Sum(x=>x.TotalWeight) + node.Weight;
}

class Node
{
	public Node Parent { get; set; }
	public string Name { get; set; }
	public int Weight { get; set; }
	public List<Node> Children { get; set; } = new List<Node>();
	public List<string> ChildrenNames { get; set; } = new List<string>();
}
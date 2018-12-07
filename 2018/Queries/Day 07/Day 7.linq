<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput().Dump();
    var data = ProcessInput(input);
    var result1 = Solve1(data).Dump("1");
	var result2 = Solve2(data).Dump("2");
}

private string Solve1(IEnumerable<Node> data)
{
    List<Node> availableNodes = data.Where(x=>x.Parents.Count == 0).ToList();
    
    List<Node> result = new List<UserQuery.Node>();
    
    while(availableNodes.Count != 0)
    {
        var nodes = availableNodes.OrderBy(x=>x.Id);
        
        var node = nodes.FirstOrDefault(x=>x.IsUsed == false && x.Parents.All(y=>y.IsUsed));
        if(node == null)
        break;
        node.IsUsed = true;
        availableNodes.Remove(node);
        result.Add(node);
        availableNodes.AddRange(node.Children.Where(x=>x.IsUsed == false));
    }
    
    return string.Join("", result.Select(x=>x.Id));
}

private string Solve2(IEnumerable<Node> data)
{
	throw new NotImplementedException();
}

private IEnumerable<Node> ProcessInput(string input)
{
    Regex regex = new Regex("Step (.) must be finished before step (.) can begin");
    var elements = input.SplitLines().Select(x => regex.Match(x)).Select(x => new { Parent = x.Groups[1].Value, Node = x.Groups[2].Value });
    
    Dictionary<string, Node> nodes = new Dictionary<string, UserQuery.Node>();
    
    foreach (var element in elements)
    {
        if (!nodes.ContainsKey(element.Node))
        {
            nodes[element.Node] = new Node() { Id = element.Node };
        }
        if (!nodes.ContainsKey(element.Parent))
        {
            nodes[element.Parent] = new Node() { Id = element.Parent };
        }
        
        var node = nodes[element.Node];
        var parent = nodes[element.Parent];
        
        node.Parents.Add(parent);
        parent.Children.Add(node);
    }
    
    return nodes.Values.ToList();
    
    
    throw new NotImplementedException();
    
}

class Node 
{
    public string Id { get; set; }
    public HashSet<Node> Children { get; set; } = new HashSet<Node>();
    public HashSet<Node> Parents {get;set;} = new HashSet<Node>();
    public bool IsUsed { get; set; }
    
}
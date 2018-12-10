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

private int Solve1(Node data)
{
	return data.Sum();
}

private int Solve2(Node data)
{
	return data.Value();
}

private Node ProcessInput(string input)
{
    Queue<int> items = new Queue<int>(input.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries).Select(y=>int.Parse(y)));
    return Parse(items);
    
	throw new NotImplementedException();
}

private Node Parse(Queue<int> items)
{
    var count = items.Dequeue();
    var metadataCount = items.Dequeue();
    
    Node n = new Node();

    for (int i = 0; i < count; i++)
    {
        n.Children.Add(Parse(items));
    }
    
    for (int i = 0; i < metadataCount; i++)
    {
        n.Metadata.Add(items.Dequeue());
    }
    return n;

}

public class Node{
    public List<Node> Children { get; set; } = new List<Node>();
    public List<int> Metadata { get; set; } = new List<int>();
    
    public int Sum()
    {
        return Metadata.Sum(x=>x) + Children.Sum(x=>x.Sum());
    }
    
    public int Value()
    {
        if(Children.Count == 0)
        return Metadata.Sum(x=>x);
        else
            return Metadata.Sum(x => { 
                var id = x-1;
                if(Children.Count > id && id >= 0)
                    return Children[x - 1].Value();
                return 0;
            });
    }
}
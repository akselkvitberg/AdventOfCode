<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
	
    var result1 = Solve1(data.players, data.finalMarble).Dump("1");
	var result2 = Solve2(data.players, data.finalMarble).Dump("2");
}

private long Solve1(long playercount, long finalMarble)
{
	long[] players = new long[playercount];

	List<long> marbles = new List<long>() { 0, 1};
	
	int currentMarble = 1;

	long currentPlayer = 0;
	for (long i = 2; i <= finalMarble; i++)
	//for (long i = 2; i < 26; i++)
	{
		if(i % 23 == 0)
		{
			currentMarble = (currentMarble - 7);
			
			if(currentMarble < 0)
				currentMarble += marbles.Count;
				
			players[currentPlayer] += marbles[currentMarble];
			players[currentPlayer] += i;

			marbles.RemoveAt(currentMarble);
			
			//players.OrderByDescending(x => x).First().Dump();
		}
		else
		{
			var insertPolong = currentMarble + 2;
			if(insertPolong > marbles.Count )
			{
				insertPolong %= marbles.Count;
			}
			marbles.Insert(insertPolong, i);
			currentMarble = marbles.IndexOf(i);
		}
		//string.Join(" ", marbles).Dump();

		currentPlayer = (currentPlayer + 1) % playercount;
	}
	
	return players.OrderByDescending(x=>x).First();
}

private long Solve2(long playercount, long finalMarble)
{
	long[] players = new long[playercount];

	Node currentNode = new Node(0);
	currentNode.Next = currentNode;
	currentNode.Previous = currentNode;

	long currentPlayer = 0;
	for (long i = 1; i <= finalMarble * 100; i++)
	{
		if(i % 23 == 0)
		{
			var marbleToRemove = currentNode.Previous.Previous.Previous.Previous.Previous.Previous.Previous;
			players[currentPlayer] += marbleToRemove.Value + i;
			
			currentNode = marbleToRemove.Next;
			
			marbleToRemove.Previous.Next = marbleToRemove.Next;
			marbleToRemove.Next.Previous = marbleToRemove.Previous;

			//players.OrderByDescending(x=>x).First().Dump();
		}
		else
		{
			var node = new Node(i);
			
			currentNode = currentNode.Next;

			node.Previous = currentNode;
			node.Next = currentNode.Next;
			node.Next.Previous = node;
			currentNode.Next = node;

			currentNode = node;
		}
		
		currentPlayer = (currentPlayer + 1) % playercount;
	}
	
	
	return players.OrderByDescending(x=>x).First();
}

private (long players, long finalMarble) ProcessInput(string input)
{
	var matches = new Regex(@"(\d+) players; last marble is worth (\d+) points").Match(input);
	return (long.Parse(matches.Groups[1].Value), long.Parse(matches.Groups[2].Value));
	return (10, 200);
	
}

class Player
{
	public long Score { get; set; }
}

class Node
{
	public Node(long value)
	{
		Value = value;
	}
	public long Value { get; set; }
	public Node Previous { get; set; }
	public Node Next { get; set; }
}
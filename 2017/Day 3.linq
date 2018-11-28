<Query Kind="Program" />

void Main()
{
	var startPos = 289326;

	var d = DoTheThing(startPos);
	var distance = Math.Abs(d.x) + Math.Abs(d.y);
	Console.WriteLine($"{startPos}: {d.x} {d.y}, distance: {distance}");
	
	var matrix = new(int x, int y) []
	{
		(0,1),
		(1,1),
		(1,0),
		(1,-1),
		(0,-1),
		(-1,-1),
		(-1,0),
		(-1,1)
	};

	Dictionary<(int x, int y), int> grid = new Dictionary<(int x, int y), int>()
	{
		[(0,0)] = 1
	};
	
	for (int i = 2; i < 100; i++)
	{
		var c = DoTheThing(i);

		var sum = 0;
		foreach (var m in matrix)
		{
			var coord = (c.x + m.x, c.y + m.y);
			grid.TryGetValue(coord, out var val);
			sum += val;
		}

		if (sum > startPos)
		{
			Console.WriteLine($"Larger number: {sum} at {c}");
			break;
		}
		
		grid[c] = sum;
	}
	
}

(int x, int y) DoTheThing(int n)
{	
	int d = (int)Math.Ceiling((Math.Sqrt(n) - 1) / 2); // Square index. This is the distance from the center to the edge of this square.
	
	int t = 2 * d + 1; // square size: 9
	
	int m = (int) Math.Pow(t, 2); // Highest number: 81
	
	t -= 1; // reduce size by one, subtract this to get number at next corner
		
	if (n >= m-t) // If number is greater than the corner, we're at the bottom row
	{
		return (x: d-(m-n), y:-d);
	}
	
	m -= t; // move highest number to currentCorner
	
	if (n >= m - t) // we're at the right side
	{
		return (-d, -d + (m - n));		
	}
	
	m -= t;
	
	if (n >= m - t) // we're at the top side
	{
		return (-d + (m - n), d);
	}
	
	return (d, d - (m - n - t)); // we're at the left side
}

// Define other methods and classes here

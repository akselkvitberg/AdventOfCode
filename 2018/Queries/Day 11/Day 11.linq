<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var data = 18;
    
    var result1 = Solve1(data).Dump("1");
	var result2 = Solve2(data).Dump("2");
}

private (int x, int y) Solve1(int data)
{
    int[,] cells = new int[301,301];

    for (int y = 1; y <= 300; y++)
    for (int x = 1; x <= 300; x++)
    {
        cells[x,y] = powerLevel(x,y, data);
    }

    var maxSum = int.MinValue;
    (int x, int y) coord = (0,0);
    
    for (int y = 1; y <= 298; y++)
    for (int x = 1; x <= 298; x++)
    {
        var sum = cells[x    , y]
                + cells[x + 1, y]
                + cells[x + 2, y]
                
                + cells[x    , y + 1]
                + cells[x + 1, y + 1]
                + cells[x + 2, y + 1]

                + cells[x    , y + 2]
                + cells[x + 1, y + 2]
                + cells[x + 2, y + 2];
        if(sum > maxSum)
        {
            maxSum = sum;
            coord = (x,y);
        }
    }

    return coord;
}

private int Solve2(int data)
{
    int[,] cells = new int[301, 301];

    for (int y = 1; y <= 300; y++)
        for (int x = 1; x <= 300; x++)
        {
            cells[x, y] = powerLevel(x, y, data);
        }

    var maxSum = int.MinValue;
    (int x, int y, int size) coord = (0, 0, 0);



    var totalSum = 0;
    {
        for (int y = 1; y <= 300; y++)
            for (int x = 1; x <= 300; x++)
            {
                totalSum += cells[x, y];
            }
    }
    int right = 1;
    int top = 1;
    int left = 300;
    int bottom = 300;
    while(true)
    {
        var topRow = totalSum;
        for (int i = right; i <= left; i++)
        {
            topRow -= cells[i, top];
        }

        var bottomRow = totalSum;
        for (int i = right; i <= left; i++)
        {
            bottomRow -= cells[i, bottom];
        }
        
        var rightColumn = totalSum;
        for (int i = top; i <= bottom; i++)
        {
            rightColumn -= cells[right, i];
        }

        var leftColumn = totalSum;
        for (int i = top; i <= bottom; i++)
        {
            leftColumn -= cells[left, i];
        }
        
        var max = Math.Max(totalSum, Math.Max(leftColumn, Math.Max(rightColumn, Math.Max(topRow, bottomRow))));
        
        if(max == leftColumn)
        {
            left--;
            totalSum = leftColumn;
        }
        else if (max == rightColumn)
        {
            right++;
            totalSum = rightColumn;
        }
        else if(max == topRow)
        {
            top++;
            totalSum = topRow;
        }
        else if(max == bottomRow)
        {
            bottom--;
            totalSum = bottomRow;
        }
        else 
        {
            return (right, top, ;
        }
        
        
        
        (right, top, left, bottom).ToString().Dump();
        
    }

    return 0;
}

int powerLevel(int x, int y, int serialNr)
{
    int id = x + 10;
    int powerLevel = y * id;
    powerLevel += serialNr;
    powerLevel *= id;
    powerLevel = powerLevel / 100 % 10;
    powerLevel-= 5;
    return powerLevel;
}
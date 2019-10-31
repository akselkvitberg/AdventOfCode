<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

async Task Main()
{
    var input = await AdventOfCode.GetInput();
    var data = ProcessInput(input);
    var result1 = Solve1(data).Dump("1");
	var result2 = Solve2(data).Dump("2");
}

private int Solve1(IEnumerable<Guard> data)
{
    var sleepyHead = data.Select(x => new
    {
        Id = x.Id,
        Minutes = x.Minutes.GroupBy(z => z).Select(z => new { z.Key, Count = z.Count() })
    }).OrderByDescending(x=>x.Minutes.Sum(y=>y.Count)).First();
    
    var minute = sleepyHead.Minutes.OrderByDescending(u=>u.Count).First();
    
    return sleepyHead.Id * minute.Key;
}

private int Solve2(IEnumerable<Guard> data)
{
    var sleepyHead = data.Select(x => new {
        Id = x.Id,
        Minutes = x.Minutes.GroupBy(z => z).Select(z => new { z.Key, Count = z.Count()}).OrderByDescending(z=>z.Count).FirstOrDefault()
    }).OrderByDescending(x=>x.Minutes.Count).First();

    return sleepyHead.Id * sleepyHead.Minutes.Key;
}

private IEnumerable<Guard> ProcessInput(string input)
{
    var lines = input.SplitLines().OrderBy(x=>x);
    Regex regex = new Regex(@"\[(\d\d\d\d-\d\d-\d\d \d\d:\d\d)] (Guard #(\d+))?(.*)");
    List<Guard> guards = new List<UserQuery.Guard>();
    Guard currentGuard = null;
    int fallAsleep = 0;
    
    foreach (var line in lines)
    {
        var match = regex.Match(line);
        var time = DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        
        var action = match.Groups[4].Value;
        
        if(action == "falls asleep")
            fallAsleep = time.Minute;
        else if (action == "wakes up")
        {
            currentGuard.Minutes = Enumerable.Range(fallAsleep, time.Minute - fallAsleep);
            currentGuard = new Guard() { Id = currentGuard.Id };
            guards.Add(currentGuard);
        }
        else
        {
            currentGuard = new Guard() { Id = int.Parse(match.Groups[3].Value) };
            guards.Add(currentGuard);
        }
    }
    return guards
        .Where(x => x.Minutes != null)
        .GroupBy(x => x.Id).Select(x => new Guard
        {
            Id = x.Key,
            Minutes = x.SelectMany(y => y.Minutes) 
        });
}

class Guard
{
    public int Id { get; set; }
    public IEnumerable<int> Minutes {get;set;}
}
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
	var sleepyHead = data
        .GroupBy(x=>x.Id)
        .Select(x=> new { x.Key, shifts = x, SleepTime = x.Sum(g=>(g.WakeUp- g.FallAsleep).TotalMinutes)})
        .OrderByDescending(x=>x.SleepTime).First();

    int[] minutes = new int[60];
    foreach (var shift in sleepyHead.shifts)
    {
        for (int i = shift.FallAsleep.Minute; i < shift.WakeUp.Minute; i++)
        {
            minutes[i]++;
        }
    }
    
    var minute = minutes.Select((x, i) => new { x, i}).OrderByDescending(x=>x.x).First().i;
    
    return minute * sleepyHead.shifts.First().Id;
}

private int Solve2(IEnumerable<Guard> data)
{
    var sleepyHeads = data.GroupBy(x=> x.Id).Select(x => new {
        Id = x.Key,
        Minutes = x.SelectMany(y => Enumerable.Range(y.FallAsleep.Minute, y.WakeUp.Minute - y.FallAsleep.Minute)).GroupBy(z => z).Select(z => new { z.Key, Count = z.Count()})
    });
    //.OrderByDescending(x=>x.Minutes.Count)


var guard = data.Where(x => x.Id == 751);
    guard.Select(x => new {S = x.FallAsleep.Minute, W= x.WakeUp.Minute, Minutes = Enumerable.Range(x.FallAsleep.Minute, x.WakeUp.Minute - x.FallAsleep.Minute)})
    .SelectMany(x=>x.Minutes)
    .Dump();

//sleepyHeads.Dump();
    var max = -1;
    var maxId = -1;
    var maxMinute = -1;
    foreach (var g in sleepyHeads)
    {
        for (int mi = 0; mi < 60; mi++)
        {
            var min = g.Minutes.FirstOrDefault(x=>x.Key == mi);
            if(min == null)
                continue;
            if (min.Count > max)
            {
                maxId = g.Id;
                max = min.Count;
                maxMinute = mi;
            }
        }
    }
    
    maxMinute.Dump();
    maxId.Dump();
    max.Dump();

    return maxMinute * maxId;
}

private IEnumerable<Guard> ProcessInput(string input)
{
    var lines = input.SplitLines();
    //lines.First().Dump();
    Regex regex = new Regex(@"\[(\d\d\d\d-\d\d-\d\d \d\d:\d\d)] (Guard #(\d+))?(.*)");

    var actions = lines.Select(x=>{
        var match = regex.Match(x);
        var action = match.Groups[4].Value;
        return new Action(){
            Time = DateTime.ParseExact(match.Groups[1].Value,"yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            ActionType = action == "falls asleep" ? ActionType.FallAsleep : action == "wakes up" ? ActionType.WakeUp : ActionType.Start,
            Id = match.Groups[3].Value
        };
    }).OrderBy(x=>x.Time).ToList();
    
    List<Guard> guards = new List<UserQuery.Guard>();
    Guard currentGuard = null;
    foreach (var action in actions)
    {
        if (action.ActionType == ActionType.Start)
        {
            currentGuard = new Guard()
            {
                Date = action.Time,
                Id = int.Parse(action.Id)
            };
            guards.Add(currentGuard);
        }
        else if (action.ActionType == ActionType.FallAsleep)
            currentGuard.FallAsleep = action.Time;
        else
        {
            currentGuard.WakeUp = action.Time;
            currentGuard = new Guard()
            {
                Date = action.Time,
                Id = currentGuard.Id
            };
            guards.Add(currentGuard);
        }
    }
    return guards;
}

class Action
{
    public DateTime Time { get; set; }
    public ActionType ActionType { get; set; }
    public string Id { get; set; }
}

enum ActionType
{
    Start,
    FallAsleep,
    WakeUp
}

class Guard
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime FallAsleep { get; set; }
    public DateTime WakeUp { get; set; }
}
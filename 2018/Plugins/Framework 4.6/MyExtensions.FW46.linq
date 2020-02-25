<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	// Write custom extension methods here. They will be available to all queries.
	public static void DoIt(){
        
    }
}

// You can also define non-static classes, enums, etc.

public static class AdventOfCode 
{
    static string sessionCookie = File.ReadAllText(Path.Combine(Util.MyQueriesFolder, "cookie.txt"));

    public static async Task<string> GetInput()
    {
        var directory = Path.GetDirectoryName(Util.CurrentQueryPath);
        var day = int.Parse(directory.Substring(directory.Length - 2));
        
        var path = Path.Combine(directory, "input.txt");
        
        if(File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        var url = new Uri($"https://adventofcode.com/2018/day/{day}/input");

        using (HttpClientHandler handler = new HttpClientHandler())
        using (HttpClient client = new HttpClient(handler))
        {
            handler.CookieContainer.Add(url, new Cookie("session", sessionCookie));
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);

            var result = await client.SendAsync(msg);
            var text = await result.Content.ReadAsStringAsync();

            File.WriteAllText(path, text);

            return text;
        }
    }
	
	public static IEnumerable<string> SplitLines(this string input)
	{
		return input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
	}

    public static string[] Split(this string str, string separator)
    {
        return str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
    }
}
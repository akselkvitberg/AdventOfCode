module Utils

open System.IO
open System.Net
open System.Net.Http
open System
open System.Text.RegularExpressions

let httpClient =
    let token = File.ReadAllText("../token")
    let cookies = new CookieContainer()
    cookies.Add(new Cookie("session", token, "/", "adventofcode.com"))
    
    let handler = new HttpClientHandler()
    handler.CookieContainer <- cookies
    
    let client = new HttpClient(handler);
    client.BaseAddress <- new Uri("https://adventofcode.com");
    client

let GetData day =
    let file = $"Inputs/{day}.txt"
    if File.Exists file then
        File.ReadAllText file
    else
        let str = httpClient.GetStringAsync($"/2017/day/{day}/input").GetAwaiter().GetResult();
        let trimmedstr = str.Trim()
        Directory.CreateDirectory "Inputs" |> ignore
        File.WriteAllText (file, trimmedstr)

        trimmedstr

let GetExample day id =
    let file = $"Examples/{day}-{id}.txt"
    if File.Exists file then
        File.ReadAllText file
    else
        Directory.CreateDirectory("Examples") |> ignore
        let html = httpClient.GetStringAsync($"/2017/day/{day}").GetAwaiter().GetResult()
        let pattern = @"<pre><code>(.*?)</code></pre>";
        let matches = Regex.Matches(html, pattern, RegexOptions.Singleline)
        let exampleData = matches |> Seq.map (fun m -> m.Groups[1].Value) |> Seq.toArray |> fun a -> Array.get a (id-1)
        let decodedExampleData = WebUtility.HtmlDecode exampleData
        File.WriteAllText (file, decodedExampleData)
        decodedExampleData

let Split (character:string) (input:string) = input.Split(character, StringSplitOptions.RemoveEmptyEntries)

let GetLines (input:string)  = input.Trim().Split([|"\n"; "\r\n" |], StringSplitOptions.RemoveEmptyEntries)
let GetBlocks (input:string) = input.Trim().Split([|"\n\n"; "\r\n\r\n"|], StringSplitOptions.RemoveEmptyEntries)
let ToLower (input:string) = input.ToLower()
let ToUpper (input:string) = input.ToUpper()

let charToInt (c:char) = int c - int '0'

let (|Regex|_|) pattern input = 
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value])
    else None

let (|Int|_|) (str:string) = match Int32.TryParse str with true, value -> Some value | _ -> None
let (|UInt|_|) (str:string) = match UInt32.TryParse str with true, value -> Some value | _ -> None
let (|Long|_|) (str:string) = match Int64.TryParse str with true, value -> Some value | _ -> None
let (|ULong|_|) (str:string) = match UInt64.TryParse str with true, value -> Some value | _ -> None

let (|IntArray|) list = 
    list |> Split " " |> Array.map int

let (|LongArray|) list = 
    list |> Split " " |> Array.map int64

let (|SplitArray|) character list = 
    list |> Split character

let (|SplitList|) character list = 
    list |> Split character |> List.ofArray

let RegexReplace pattern (replacement:string) input =
    let regex = new Regex(pattern)
    regex.Replace(input, replacement)

let Match pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then List.tail [ for g in m.Groups -> g.Value]
    else []

let Matches pattern input =
    let m = Regex.Matches(input, pattern)
    m |> Seq.collect (fun x -> x.Groups |> Seq.collect (fun y -> y.Captures |> Seq.map (fun z -> z.Value)) |> Seq.skip 1 |> Seq.toList) |> Seq.toList

open System.IO
open System.Net
open System.Net.Http
open System
open System.Text.RegularExpressions

let private year = Directory.GetCurrentDirectory() |> Path.GetFileName

let private get (path: string) =
    let token = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "token"))

    let cookies = new CookieContainer()
    cookies.Add(new Cookie("session", token, "/", "adventofcode.com"))

    let handler = new HttpClientHandler()
    handler.CookieContainer <- cookies

    let client = new HttpClient(handler)
    client.BaseAddress <- new Uri "https://adventofcode.com"
    let result = client.GetStringAsync(path).GetAwaiter().GetResult()
    result.Trim()

let GetData (day: int) =
    let directory = Path.Combine(__SOURCE_DIRECTORY__, string year, "Inputs")
    let file: string = Path.Combine(directory, $"{day}.txt")

    if File.Exists file then
        File.ReadAllText file
    else
        let str = get $"/{year}/day/{day}/input"
        Directory.CreateDirectory directory |> ignore
        File.WriteAllText(file, str)
        str

let GetExample (day: int) (id: int) =
    let directory = Path.Combine(__SOURCE_DIRECTORY__, string year, "Examples")
    let file = Path.Combine(directory, $"{day}-{id}.txt")

    if File.Exists file then
        File.ReadAllText file
    else
        let html = get $"/{year}/day/{day}"
        let pattern = @"<pre><code>(.*?)</code></pre>"
        let matches = Regex.Matches(html, pattern, RegexOptions.Singleline)

        let exampleData =
            matches
            |> Seq.map (fun m -> m.Groups[1].Value)
            |> Seq.toArray
            |> fun a -> Array.get a (id - 1)

        let decodedExampleData = WebUtility.HtmlDecode exampleData
        Directory.CreateDirectory directory |> ignore
        File.WriteAllText(file, decodedExampleData.Trim())
        decodedExampleData

/// Split a string by a character, removing empty entries
let Split (character: string) (input: string) =
    input.Split(character, StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)

/// Split input into lines, removing empty entries
let GetLines (input: string) =
    input.Trim().Split([| "\n"; "\r\n" |], StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)

/// Split input into blocks separated by double newlines
let GetBlocks (input: string) =
    input
        .Trim()
        .Split([| "\n\n"; "\r\n\r\n" |], StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)

/// Convert string to lowercase
let ToLower (input: string) = input.ToLower()

/// Convert string to uppercase
let ToUpper (input: string) = input.ToUpper()

/// Convert character to integer (0-9)
let charToInt (c: char) = int c - int '0'

/// Active pattern for regex matching that returns capture groups
let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)

    if m.Success then
        Some(List.tail [ for g in m.Groups -> g.Value ])
    else
        None

/// Active pattern for regex matching that returns capture groups and the match object
let (|RegexEx|_|) pattern input =
    let m = Regex.Match(input, pattern)

    if m.Success then
        Some(List.tail [ for g in m.Groups -> g.Value ], m)
    else
        None

/// Active pattern for parsing integers
let (|Int|_|) (str: string) =
    match Int32.TryParse str with
    | true, value -> Some value
    | _ -> None

/// Active pattern for parsing unsigned integers
let (|UInt|_|) (str: string) =
    match UInt32.TryParse str with
    | true, value -> Some value
    | _ -> None

/// Active pattern for parsing long integers
let (|Long|_|) (str: string) =
    match Int64.TryParse str with
    | true, value -> Some value
    | _ -> None

/// Active pattern for parsing unsigned long integers
let (|ULong|_|) (str: string) =
    match UInt64.TryParse str with
    | true, value -> Some value
    | _ -> None

/// Active pattern for splitting string by spaces and converting to int array
let (|IntArray|) list = list |> Split " " |> Array.map int

/// Active pattern for splitting string by spaces and converting to long array
let (|LongArray|) list = list |> Split " " |> Array.map int64

/// Active pattern for splitting string by specified character into array
let (|SplitArray|) character list = list |> Split character

/// Active pattern for splitting string by specified character into list
let (|SplitList|) character list = list |> Split character |> List.ofArray

/// Replace text in a string
let Replace (string: string) (oldValue: string) (newValue: string) = string.Replace(oldValue, newValue)

/// Replace text using regex pattern
let RegexReplace pattern (replacement: string) input =
    let regex = new Regex(pattern)
    regex.Replace(input, replacement)

/// Get first regex match groups as list
let Match pattern input =
    let m = Regex.Match(input, pattern)

    if m.Success then
        List.tail [ for g in m.Groups -> g.Value ]
    else
        []

/// Get all regex matches as flattened list of capture groups
let Matches pattern input =
    let m = Regex.Matches(input, pattern)

    m
    |> Seq.collect (fun x ->
        x.Groups
        |> Seq.collect (fun y -> y.Captures |> Seq.map (fun z -> z.Value))
        |> Seq.skip 1
        |> Seq.toList)
    |> Seq.toList

module Utils

open System.IO
open System.Net
open System.Net.Http
open System

let GetData day =
    let file = $"Inputs/{day}.txt"
    if File.Exists file then
        File.ReadAllText file
    else
        let token = File.ReadAllText("../token")
        
        let cookies = new CookieContainer()
        cookies.Add(new Cookie("session", token, "/", "adventofcode.com"))
        
        let handler = new HttpClientHandler()
        handler.CookieContainer <- cookies
        
        let client = new HttpClient(handler);
        client.BaseAddress <- new Uri("https://adventofcode.com");
        
        let str = client.GetStringAsync($"/2016/day/{day}/input").GetAwaiter().GetResult();
        
        Directory.CreateDirectory "Inputs" |> ignore
        File.WriteAllText (file, str)

        str

let Split (character:string) (input:string) = input.Split(character)
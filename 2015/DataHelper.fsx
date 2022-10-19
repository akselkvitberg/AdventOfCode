module DataHelper

open System
open System.IO
open System.Net
open System.Net.Http

let LoadInputFile file = File.ReadAllText(file)

let DownloadInput day file =
    let token = File.ReadAllText "../token"
    
    let uri = Uri "https://adventofcode.com"
    
    let cc = CookieContainer()
    let cookie = Cookie ("session", token)
    cc.Add(uri, cookie)

    let httpClientHandler = new HttpClientHandler()
    httpClientHandler.CookieContainer <- cc
    let client = new HttpClient(httpClientHandler)
    let result = client.GetAsync($"https://adventofcode.com/2015/day/{day}/input").Result
    result.Content.ReadAsStringAsync().Result

let LoadDay day =
    let inputFilePath = Path.Combine("input", $"{day}.txt");
    if (File.Exists inputFilePath)
        then LoadInputFile inputFilePath
    else
        DownloadInput inputFilePath day

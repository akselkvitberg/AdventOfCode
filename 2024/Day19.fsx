// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 19
let example = GetExample 19 1
example

// %%
let parse input =
    let [|towels; patterns|] =
        input
        |> GetBlocks
    
    let towels = 
        towels
        |> Split ", "
    
    let patterns =
        patterns
        |> GetLines

    towels, patterns

parse example

// %%
open System.Collections.Generic
let patternExists (towels:string array) (pattern:string) =
    let rec loop pattern =
        if pattern = "" then true
        else 
            towels
            |> Array.filter (pattern.StartsWith)
            |> Array.exists (fun towel -> loop (pattern.Substring(towel.Length)))

    loop pattern

patternExists [|"r";  "wr"; "b"; "g"; "bwu"; "rb"; "gb"; "br"|] "rrbgbr"

// %%
let part1 input =
    let towels, patterns = parse input

    patterns
    |> Array.filter (patternExists towels)
    |> Array.length

part1 example

// %%
part1 input

// %%
open System.Collections.Generic
let getAllPatterns (towels:string array) (pattern:string) =
    let cache = Dictionary<string, int64>()
    
    let rec loop pattern acc =
        match cache.TryGetValue(pattern), pattern with 
        | (true, result), _ -> result
        | _, "" -> acc + 1L
        | _ -> 
            let sum = 
                towels
                |> Seq.filter (pattern.StartsWith)
                |> Seq.sumBy (fun towel -> loop (pattern.Substring(towel.Length)) acc)
            cache.Add(pattern, sum) |> ignore
            sum

    loop pattern 0L

getAllPatterns [|"r";  "wr"; "b"; "g"; "bwu"; "rb"; "gb"; "br"|] "rrbgbr"

// %%
let part2 input =
    let towels, patterns = parse input

    patterns
    |> Seq.map (getAllPatterns towels)
    |> Seq.sum

part2 example

// %%
part2 input

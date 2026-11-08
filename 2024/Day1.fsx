// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 1
let example = GetExample 1 1

// %%
let parseList input = 
    input
    |> GetLines
    |> Array.map (fun x -> Split " " x |> Array.map int)
    |> Array.transpose
    |> fun [|a;b|] -> a,b

parseList example

// %%
let part1 (list1, list2) =
    let list1 = list1 |> Array.sort
    let list2 = list2 |> Array.sort

    list1
    |> Array.zip list2
    |> Array.sumBy (fun (a,b) -> abs(a - b))

parseList example |> part1    

// %%
parseList input |> part1

// %%
let part2 (list1, list2) =
    let appearances = list2 |> Array.countBy id |> Map.ofArray
    
    list1
    |> Array.sumBy (fun x -> x * (appearances.TryFind(x) |> Option.defaultValue 0))


parseList example |> part2

// %%
parseList input |> part2

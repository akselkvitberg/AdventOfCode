// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 5
let example = GetExample 5 1
example

// %%
let parse input = 
    let [| rules; updates |] = Split "\n\n" input |> Array.map GetLines

    let rules = rules   |> Array.map (Split "|" >> fun [|Int a; Int b|] -> a,b) |> Set
    let updates = updates |> Array.map (Split "," >> Array.map int >> Array.toList)

    rules, updates

parse example

// %%
let sort (rules: Set<int*int>) update = 
    update |> List.sortWith (fun a b -> if rules.Contains (a,b) then -1 else 1)

// %%
let part1 input = 
    let rules, updates = parse input
    updates
    |> Array.filter (fun pages -> pages = sort rules pages)
    |> Array.sumBy (fun pages -> pages[pages.Length/2])
part1 example

// %%
part1 input

// %%
let part2 input = 
    let rules, updates = parse input
    updates
    |> Array.choose (fun pages -> 
        let sorted = sort rules pages
        if sorted = pages then None else Some sorted)
    |> Array.sumBy (fun pages -> pages[pages.Length/2])
    
part2 example

// %%
part2 input

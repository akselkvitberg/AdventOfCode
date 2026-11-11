// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
open System.Text.RegularExpressions

let input = GetData 3 |> GetLines

// %%
input |> Seq.collect (fun x -> x |> Seq.map id) |> Seq.distinct |> Seq.toArray |> String

// %%
let example = GetExample 3 1 |> GetLines
example

// %%
let getNeighbors x1 x2 y (input: string array) =
    let adjacent = 
        [x1-1..x2+1] 
        |> List.collect (fun x -> [(x, y+1); (x, y-1); (x,y)])
        |> List.where (fun (x,y) -> x >= 0 && x < input.[0].Length && y >= 0 && y < input.Length)
        |> List.exists (fun (x,y) -> match input.[y].[x] with | '*' | '/' | '+' |'%'|'&'|'-'|'='|'$'|'@'|'#' -> true | _ -> false)
    adjacent

getNeighbors 5 7 0 example

// %%
let regex = Regex(@"\d+")

let getNumbersWithAdjecantSymbols input =
    input
    |> Array.mapi (fun y l -> regex.Matches(l) |> Seq.map (fun m -> m.Value, m.Index, m.Index + m.Length-1, y) |> Seq.toArray)
    |> Array.collect id
    |> Array.where (fun (v, x1, x2, y) -> getNeighbors x1 x2 y input)
    |> Array.sumBy (fun (v, _, _, _) -> int v)

getNumbersWithAdjecantSymbols example

// %%
getNumbersWithAdjecantSymbols input

// %%
let getNeighborGear x1 x2 y (input: string array) =
    let adjacent = 
        [x1-1..x2+1] 
        |> List.collect (fun x -> [(x, y+1); (x, y-1); (x,y)])
        |> List.where (fun (x,y) -> x >= 0 && x < input.[0].Length && y >= 0 && y < input.Length)
        |> List.tryPick (fun (x,y) -> match input.[y].[x] with | '*' -> Some (x,y) | _ -> None)
    adjacent

getNeighborGear 1 3 0 example

// %%
let getNumbersWithAdjecantSymbols input =
    input
    |> Array.toList
    |> List.mapi (fun y l -> regex.Matches(l) |> Seq.map (fun m -> int m.Value, m.Index, m.Index + m.Length-1, y) |> Seq.toList)
    |> List.collect id
    |> List.choose (fun (v, x1, x2, y) -> match getNeighborGear x1 x2 y input with | Some pos -> Some (v, pos) | _ -> None)
    |> List.groupBy snd
    |> List.map snd
    |> List.sumBy (function [(a,_);(b,_)] -> a * b | _ -> 0)

getNumbersWithAdjecantSymbols example

// %%
getNumbersWithAdjecantSymbols input

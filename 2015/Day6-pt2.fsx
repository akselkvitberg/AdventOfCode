// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.Text

// %%
open System.Text.RegularExpressions
open System.IO

#load "../utils.fsx"
open Utils
let input = GetData 6 |> GetLines

// %%
let Increase i = i + 1
let Decrease i = max 0 (i - 1)
let Toggle i = i + 2

let Apply (position:int*int) func grid = grid |> Map.change position (fun x -> 
    match x with
    | Some v -> Some (func v)
    | None -> Some (func 0)
    )

// %%
let Range (line:string) = 
    let ints = [| for m in Regex.Matches(line, "(-?\d+)") -> int m.Value |]
    let xs = [ints[0]..ints[2]]
    let ys = [ints[1]..ints[3]]
    xs |> Seq.allPairs ys
Range "turn off 10,10 through 12,12"

// %%
let Parse (line:string) = 
    if line.StartsWith("toggle") then Toggle
    elif line.StartsWith("turn on") then Increase
    else Decrease

// %%
let map = Map.empty<(int*int), int>
let func = Parse "turn off"
let range = Range "turn off 10,10, through 12,12"

let operations = range |> Seq.map (fun pos -> Apply pos func)

let apply map func = map |> func

operations |> Seq.fold apply map

// %%
let GetOperations line =
    let func = Parse line
    let range = Range line
    range |> Seq.map (fun pos -> Apply pos func)

let operations = input |> Seq.collect GetOperations

let map = Map.empty<(int*int), int>
operations |> Seq.fold apply map |> Map.values |> Seq.sum

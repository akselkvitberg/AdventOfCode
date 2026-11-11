// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 15 |> GetLines

// %%
let Parse line = [| for m in RegularExpressions.Regex.Matches(line, "(-?\d+)") -> int m.Value |]
let stats = input |> Array.map Parse
stats

// %%
//let getPartitions = 
[| for m in [0..100] -> (m, 100-m) |]

// %%
let rec GetPartitions =
    seq {
        for a in [0..100] do
        for b in [0..100-a] do
        for c in [0..100-a-b] do
        yield [a;b; c; 100 - a - b - c]
    }
let partitions = GetPartitions
partitions |> Seq.take 4

// %%
let ScoreCookie (amounts:int list) =
    stats |> Seq.mapi (fun i s -> s |> Array.map ((*) amounts[i])) |> Seq.reduce (Array.map2 (+))

let TotalScore (scores: int array) =
     scores |> Seq.take 4 |> Seq.reduce (fun acc x -> acc * (max 0 x))

ScoreCookie [20; 20; 25; 20] |> TotalScore

// %%
partitions |> Seq.map ScoreCookie |> Seq.map TotalScore |> Seq.max

// %%
partitions |> Seq.map ScoreCookie |> Seq.filter (fun x -> x[4] = 500) |> Seq.map TotalScore |> Seq.max

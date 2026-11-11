// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 14 |> GetLines

// %%
type Reindeer = Reindeer of string*int*int*int

// %%
let Parse (line:string) =
    let seg = line.Split(" ")
    let name = seg[0]
    let speed = int seg[3]
    let time = int seg[6]
    let rest = int seg[^1]
    Reindeer(name, speed, time, rest)

Parse input[1]
let reindeers = input |> Seq.map Parse |> Seq.toList

// %%
let Race seconds (Reindeer(_, speed, time, rest)) = 
    let intervals = time + rest
    let rounds = seconds / intervals
    let extraTime = seconds - rounds * intervals
    (min extraTime time) * speed + rounds * time * speed

Race 1000 (Reindeer("Comet", 14, 10, 127))

// %%
reindeers |> Seq.map (Race 2503) |> Seq.max

// %%
let GetDistances reindeer = [0..2503] |> Seq.map (fun sec -> Race sec reindeer) |> Seq.toArray
GetDistances (Reindeer("Comet", 14, 10, 127)) |> Seq.take 5

// %%
let distances = reindeers |> Seq.map GetDistances |> Seq.toArray

let getScoringReinder second =
    let max = distances |> Seq.map (fun x -> x[second]) |> Seq.max
    distances
    |> Seq.indexed
    |> Seq.filter (fun (i,x) -> x[second] = max )
    |> Seq.map fst

[1..1000] |> Seq.collect getScoringReinder |> Seq.length

// %%
[1..2502]
|> Seq.collect getScoringReinder
|> Seq.countBy id
|> Seq.maxBy snd

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 9 |> GetLines

// %%
let Parse  (map:Map<(string*string), int>) (input:string) =
    let parts = input.Split(" ")
    map 
    |> Map.add (parts[0], parts[2]) (int parts[4])
    |> Map.add (parts[2], parts[0]) (int parts[4])

let GetConnections input = 
    input |> Seq.fold Parse Map.empty

let connections = GetConnections input
connections |> Seq.take 5

// %%
let cities = Map.keys connections |> Seq.map fst |> Seq.distinct |> Seq.toList
cities

// %%
let rec insertions x = function
    | []             -> [[x]]
    | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

let rec permutations = function
    | []      -> seq [ [] ]
    | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))

let routes = permutations cities |> Seq.toList
routes[0..4]

// %%
let getCost route =
    route 
    |> Seq.pairwise 
    |> Seq.map connections.TryGetValue
    |> Seq.sumBy snd

routes |> Seq.map getCost |> Seq.min

// %%
routes |> Seq.map getCost |> Seq.max

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 12
let example = GetExample 12 1
example

// %%
let parse input =
    input
    |> GetLines
    |> Array.map (
        function
        | Regex @"(\d+) <-> (.+)" [ node; connections ] ->
            node, connections |> Split "," |> Array.toList
    )
    |> Map.ofArray

parse example

// %%
let traverse (startNode: string) (graph: Map<string, string list>) =
    let rec visit (visited:Set<string>) (toVisit: string list) =
        match toVisit with
        | [] -> visited
        | node :: rest ->
            let neighbors = 
                graph[node]
                |> List.filter (fun n -> not (Set.contains n visited))
            let newToVisit = rest @ neighbors
            let newVisited = Set.add node visited
            visit newVisited newToVisit
    visit Set.empty [ startNode ]

parse example
|> traverse "0"  

// %%
let sizeOfGraph input =
    parse input
    |> traverse "0"
    |> Set.count

sizeOfGraph example

// %%
sizeOfGraph input

// %%
let getDistinctGroups (graph: Map<string, string list>) =
    graph.Keys
    |> Seq.distinctBy (fun node -> traverse node graph)
    |> Seq.length

parse example
|> getDistinctGroups

// %%
parse input
|> getDistinctGroups

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 4
let example = GetExample 4 1
example

// %%
let parse input = 
    input
    |> GetLines
    |> Seq.mapi (fun y line -> 
        line 
        |> Seq.mapi (fun x c -> ((x,y), c = '@'))
        |> Seq.filter snd
        |> Seq.map fst)
    |> Seq.collect id
    |> Set.ofSeq
//parse example

// %%
let getNeighbors (x,y) =
    [ 
        (x-1,y)
        (x-1,y-1)
        (x-1,y+1)
        (x+1,y)
        (x+1,y-1)
        (x+1,y+1)
        (x,y-1)
        (x,y+1)
    ]
    |> Set.ofList

// %%
let countNeighbors grid pos =
    pos
    |> getNeighbors
    |> fun u -> Set.intersect u grid // Observation - doing Set.intersect grid u is enormously slower than Set.intersect u grid due to the sizes of the sets
    |> Set.count

// %%
let getAccessibleRolls input =
    let grid = parse input
    grid
    |> Set.filter (fun pos -> countNeighbors grid pos < 4)
    |> Set.count
getAccessibleRolls example

// %%
getAccessibleRolls input

// %%
let getAccessible grid =
    grid |> Set.filter (fun pos -> countNeighbors grid pos < 4)

let countAllRemovableRolls input =
    let grid = parse input
    let rec loop currentGrid acc =
        let accessible = getAccessible currentGrid
        if Set.isEmpty accessible then
            acc
        else
            let newGrid = Set.difference currentGrid accessible
            loop newGrid (acc + Set.count accessible)
    loop grid 0

countAllRemovableRolls example

// %%
countAllRemovableRolls input

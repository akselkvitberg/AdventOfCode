#load "../utils.fsx"
open Utils

let input = GetData 22
let example = GetExample 22 1
example

// %%
let parse input = PositionsOf '#' input
parse input
// %%
let turnRight (dx, dy) = (-dy, dx)
let turnLeft  (dx, dy) = (dy, -dx)
let move (x,y) (dx,dy) = (x + dx, y+dy)

let burst (infectedNodes, carrierPos, carrierDir, count)=
    // If the current node is infected, it turns to its right. Otherwise, it turns to its left.
    let isInfected = Set.contains carrierPos infectedNodes
    let carrierDir = if isInfected then turnRight carrierDir else turnLeft carrierDir
    
    // If the current node is clean, it becomes infected. Otherwise, it becomes cleaned.
    let infectedNodes = (if isInfected then Set.remove carrierPos else Set.add carrierPos) infectedNodes
    let count = if isInfected then count else count+1

    // The virus carrier moves forward one node in the direction it is facing.
    let carrierPos = move carrierPos carrierDir
    
    infectedNodes, carrierPos, carrierDir, count

// %%
let part1 input =
    let infectedNodes = parse input
    let size = (GetLines input).Length
    let start = (size / 2, size / 2)
    [1..10000]
    |> List.fold (fun state _ -> burst state) (infectedNodes, start, (0,-1), 0)
    |> fun (_, _, _, count) -> count

part1 example

// %%
part1 input

// %% Part 2
type NodeState = Clean | Weakened | Infected | Flagged
let reverse (dx, dy) = (-dx, -dy)
open System.Collections.Generic

let burst2 (nodes: Dictionary<struct (int * int), NodeState>) ((x,y), carrierDir, count) =
    let key = struct (x, y)
    let nodeState = match nodes.TryGetValue key with | true, s -> s | _ -> Clean

    let carrierDir =
        match nodeState with
        | Clean -> turnLeft carrierDir
        | Weakened -> carrierDir
        | Infected -> turnRight carrierDir
        | Flagged -> reverse carrierDir

    let newState =
        match nodeState with
        | Clean -> Weakened
        | Weakened -> Infected
        | Infected -> Flagged
        | Flagged -> Clean

    nodes[key] <- newState
    let count = if newState = Infected then count + 1 else count

    let carrierPos = move (x,y) carrierDir

    carrierPos, carrierDir, count

let part2 input =
    // Map - 30 s
    // Map with hashed key - 5 s
    // Dictionary - 3 s
    // Dictionary with struct key - 0.7 s
    // Also replace fold with inline loop and mutables - 0.5 s
    let nodes = Dictionary<struct (int * int), NodeState>()
    for (x, y) in parse input do nodes[struct (x, y)] <- Infected

    let size = (GetLines input).Length
    let start = (size / 2, size / 2)
    seq { 1..10_000_000 }
    |> Seq.fold (fun state _ -> burst2 nodes state) (start, (0, -1), 0)
    |> fun (_, _, count) -> count

part2 example
// %%
part2 input
#load "../utils.fsx"
open Utils

let input = GetData 22
let example = """depth: 510
target: 10,10"""
example

// %%

/// depth, (targetX, targetY)
let parse input =
    match Matches @"(\d+)" input |> List.map int with
    | [ depth; x; y ] -> depth, (x, y)
    | numbers -> failwith $"Expected depth, x and y, got %A{numbers}"

parse example

// %%
type Type = Rocky | Wet | Narrow
let getType erosionLevel =
    match erosionLevel % 3 with
    | 0 -> Rocky
    | 1 -> Wet
    | _ -> Narrow

let buildErosionLevels depth (tx, ty) margins =
    let width, height = tx + margins, ty + margins
    let erosion = Array2D.zeroCreate<int> (height+1) (width+1)
    for y in 0..height do
        for x in 0..width do
            let geologicIndex = match (x,y) with
                                | 0,0 -> 0
                                | x,y when x = tx && y = ty -> 0
                                | x,0 -> x * 16807
                                | 0,y -> y * 48271
                                | _ -> erosion[y-1,x] * erosion[y, x-1]
            erosion[y,x] <- (geologicIndex + depth) % 20183
    
    erosion
    
buildErosionLevels 510 (10,10) 1

// %%
let part1 input =
    let depth, (tx, ty) = parse input
    let erosion = buildErosionLevels depth (tx,ty) 0
    erosion |> Seq.cast<int> |> Seq.sumBy (fun e -> e % 3)
    
part1 example

// %%
part1 input

// %%
type Tool = Torch | Gear | Neither
type State = (int * int * Tool)

let isAllowed regionType tool =
    match regionType, tool with
    | Rocky, (Gear | Torch) -> true // i did not know f# could do this. wow
    | Wet, (Gear | Neither) -> true
    | Narrow, (Torch | Neither) -> true
    | _ -> false

let allowedStates regionAt (x,y,tool) =
    let directions = [ x, y - 1; x - 1, y; x + 1, y; x, y + 1 ]
    [
        let here = (regionAt (x,y)) |> Option.get 
        for other in [ Torch; Gear; Neither ] do
            if other <> tool && isAllowed here other then (7, (x, y, other))
        for nx, ny in directions do
            match regionAt (nx, ny) with
            | Some region when isAllowed region tool -> 1, (nx, ny, tool)
            | _ -> () ]

isAllowed Rocky Gear

// %%
// dijkstra

module PriorityQueue =
    let pop (queue: Set<int * (State)>) : int * (State) * Set<int * (State)> =
        let time, (x,y,tool) as current = Set.minElement queue
        let queue = Set.remove current queue
        time, (x,y,tool), queue
    
    let add next queue =
        Set.union queue (Set.ofList next)
    
    let initial = Set.singleton (0, (0,0,Torch))

let find erosion target =
    let regionAt (x, y) =
        if x >= 0 && y >= 0 && y < Array2D.length1 erosion && x < Array2D.length2 erosion
        then Some (getType erosion[y, x])
        else None
        
    let rec search queue visited =
        let time, (x,y,tool), queue = PriorityQueue.pop queue
        
        if (x, y) = target && tool = Torch then time // duh - "you need the torch equipped before you can find him in the dark"
        elif Set.contains (x, y, tool) visited then search queue visited
        else
            let visited = Set.add (x,y,tool) visited
            let next = allowedStates regionAt (x,y,tool) |> List.map (fun (cost, state) -> time + cost, state)
            search (PriorityQueue.add next queue) visited
    
    search PriorityQueue.initial Set.empty 

// %%
let part2 input =
    let (depth, target) = parse input
    let erosion = buildErosionLevels depth target 75
    find erosion target

part2 example

// %%
part2 input
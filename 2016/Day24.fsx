// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 24
let example = GetExample 24 1
example
//input

// %%
type CellType =
    | Wall
    | Space
    | Target of int

type Grid = Map<(int * int), CellType>

let parse input =
    input
    |> GetLines
    |> Array.mapi (fun y line -> 
        line.ToCharArray()
        |> Array.mapi (fun x c ->
            (x,y), 
            match c with
            | '#' -> Wall
            | '.' -> Space
            | i -> Target (int (string i))))
    |> Array.collect id
    |> Map.ofArray
//parse example

// %%
type Queue<'a> = 'a list * 'a list
let emptyQueue = [],[]
let enqueue item ((fs, bs):Queue<'a>) = item::fs, bs
let enqueueMany items queue = List.fold (fun q item -> enqueue item q) queue items
let rec dequeue queue =
    match queue with
    | [],[] -> None, ([],[])
    | fs, [] -> dequeue ([],List.rev fs)
    | fs, b::bs -> Some b, (fs, bs)

// %%
let getDistancesToTargets (grid: Grid) from fromId =
    let rec search queue visited (acc: ((int * int) * int) list) =
        match dequeue queue with
        | None, _ -> acc
        | Some (pos, steps), queue -> 
            let x, y = pos
            let cell = grid.[pos]            
            match cell with
            | Wall -> search queue visited acc
            | Space when Set.contains pos visited -> search queue visited acc
            | _ ->
                let acc = 
                    match cell with
                    | Target id when pos <> from -> ((fromId, id), steps)::acc
                    | _ -> acc
                let nextSteps = [x, y+1; x+1, y; x, y-1; x-1, y] |> List.filter (fun p -> grid.ContainsKey p)
                let queue' = enqueueMany (List.map (fun p -> p, steps + 1) nextSteps) queue
                let visited' = visited |> Set.add (x, y)
                search queue' visited' acc

    search (enqueue (from, 0) emptyQueue) Set.empty []
    |> List.distinctBy fst

getDistancesToTargets (parse example) (1,1) 0

// %%
let getAllDistances (grid: Grid) =
    let targets = 
        grid
        |> Map.toList
        |> List.choose (fun (pos, cell) ->
            match cell with
            | Target id -> Some (id, pos)
            | _ -> None)
        |> List.map (fun (id, pos) -> getDistancesToTargets grid pos id)

    targets
    |> List.collect id
    |> Map.ofList

getAllDistances (parse example)

// %%
let getDistance distances fromId toId =
    distances
    |> Map.find (fromId, toId)

let rec permutations list =
    match list with
    | [] -> [ [] ]
    | xs -> [ 
        for item in xs do
        for perm in permutations (List.filter ((<>) item) xs) do
        yield item :: perm ]

let getPermutations distances returnToStart =
    distances
    |> Map.keys
    |> Seq.map fst
    |> Seq.distinct
    |> Seq.toList
    |> List.sort
    |> List.except [ 0 ]
    |> permutations
    |> List.map (fun perm -> 
        if returnToStart 
        then 0 :: perm @ [0]
        else 0 :: perm)

let solveTSP returnToStart input =
    let distances = parse input |> getAllDistances
    let getDistance (fromId, toId) = getDistance distances fromId toId
    getPermutations distances returnToStart
    |> List.map (List.pairwise >> List.sumBy getDistance)
    |> List.min

let part1 input = solveTSP false input

part1 example

// %%
part1 input

// %%
let part2 input = solveTSP true input
part2 example

// %%
part2 input

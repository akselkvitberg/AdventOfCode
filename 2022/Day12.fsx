// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 12 |> GetLines
input[0..3]

// %%
let example =
    """
    Sabqponm
    abcryxxl
    accszExk
    acctuvwj
    abdefghi
    """
    |> GetLines
    |> Array.map (fun x -> x.Trim())

example

// %%
let parseChar char =
    match char with
    | 'S' -> -1
    | 'E' -> int 'z' - (int 'a' - 1)
    | c -> int c - int 'a'

let parseLine line = 
    line
    |> Seq.map parseChar
    |> Seq.toArray

parseLine example[0]

// %%
module Array2D =
    let toArray (arr: 'T [,]) = arr |> Seq.cast<'T> |> Seq.toArray

let parse input =
    input
    |> Seq.map parseLine
    |> Seq.toArray
    |> array2D
    |> Array2D.mapi (fun x y i -> ((x,y), i))
    |> Array2D.toArray
    |> Map.ofArray

printfn "%A" (parse example)

// %%
let getEndpoints terrain = 
    terrain |> Map.toSeq |> Seq.minBy snd |> fst, terrain |> Map.toSeq |> Seq.maxBy snd |> fst

example
|> parse
|> getEndpoints

// %%
example |> parse |> Map.find (2,5)

// %%
let isNeighbour (x,y) (x',y') =
    match abs (x' - x), abs(y' - y) with
    | (1,0) -> true
    | (0, 1) -> true
    | _ -> false

let findTraversableNeighbours terrain pos =
    let add (a1, a2) (b1, b2) = 
        a1 + b1, a2 + b2

    match terrain |> Map.tryFind pos with 
    | None -> []
    | Some height -> 
        [(0,1); (1,0); (0,-1); (-1,0)] 
        |> Seq.map (add pos)
        |> Seq.choose (fun p -> Map.tryFind p terrain |> Option.filter (fun v -> v >= height - 1) |> Option.map (fun _ -> p))
        |> Seq.toList

findTraversableNeighbours (example |> parse) (0,0)

// %%
let findShortestPath terrain =

    let (start, stop) = getEndpoints terrain

    let terrain' = terrain |> Map.add start 0 |> Map.add stop 25 // bring start and stop down
    let mutable count = 0

    let rec propagateDistance distances positions =
        count <- count + 1
        match positions with 
        | [] -> distances
        | (pos, distance)::xs ->
            let existingDistance = distances |> Map.tryFind pos
            match existingDistance with 
            | Some d when d <= distance -> propagateDistance distances xs
            | _ ->
                let newDistanceMap = distances |> Map.add pos distance
                let neighbours = findTraversableNeighbours terrain' pos |> Seq.map (fun pos -> (pos, distance + 1))
                let neighborsToPropagate = neighbours |> Seq.filter (fun (pos, dist) -> newDistanceMap.ContainsKey(pos) |> not || newDistanceMap.TryFind(pos) |> Option.exists (fun v -> v > dist)) |> Seq.toList
                propagateDistance newDistanceMap (neighborsToPropagate |> List.append xs)

    let distances = propagateDistance Map.empty [(stop, 0)]
    //printf $"Unnessesary counts {count}"
    distances[start]

findShortestPath (example |> parse)

// %%
findShortestPath (input |> parse)

// %%
let findBetterShortestPath input =
    let terrain = input |> parse
    let (start, stop) = getEndpoints terrain

    let terrain' = terrain |> Map.add start 0
    let startingPositions = terrain' |> Map.filter (fun k v -> v = 0) |> Map.toSeq

    seq {
        for (pos, _) in startingPositions do
            //let t = Map.add pos -1 terrain'
            yield (pos, findShortestPath terrain)
    }

findBetterShortestPath example

// %%
//findBetterShortestPath input
//|> Seq.length

// %%
//input |> parse |> Map.count

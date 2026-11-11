// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 14 |> GetLines
input[..3]

// %%
let example = [|
    "498,4 -> 498,6 -> 496,6"
    "503,4 -> 502,4 -> 502,9 -> 494,9"
|]

// %%
type Block = Air | Rock | Sand

// %%
let parsePoint input=
    let [Int x; Int y] = Match "(\d+),(\d+)" input
    x,y

let getLine ((x1,y1),(x2, y2)) =
    if x1 = x2
    then [|(min y1 y2)..(max y1 y2)|] |> Array.map (fun y -> x1,y)
    else [|(min x1 x2)..(max x1 x2)|] |> Array.map (fun x -> x,y1)

let parseLine input =
    input
    |> Split "->"
    |> Array.map parsePoint
    |> Array.pairwise
    |> Array.collect getLine

let parse input =
    input
    |> Seq.collect parseLine
    |> Seq.map (fun pos -> pos,Rock)
    |> Map.ofSeq

//parse example

// %%
let renderMap map = 
    let mutable parsed = ""
    let xs = map |> Map.keys |> Seq.map fst |> Seq.toArray
    let xMin = xs |> Seq.min
    let xMax = xs |> Seq.max
    let yMax = map |> Map.keys |> Seq.map snd |> Seq.max
    
    let renderPos x y =
        match (map |> Map.tryFind (x,y) |> Option.defaultValue Air) with
        | Air -> " "
        | Rock -> "█"
        | Sand -> "o"

    let renderLine y =
        [|xMin..xMax|]
        |> Array.map (fun x -> renderPos x y)
        |> String.concat ""

    [|0..yMax|]
    |> Array.map renderLine
    |> String.concat "\n"

example
|> parse
|> Map.add (500, 0) Sand
|> Map.add (500, 1) Sand
|> renderMap

// %%
let dropSand map =
    let yMax = map |> Map.keys |> Seq.map snd |> Seq.max

    let rec settleSand (x, y) map = 
        let isFree pos = Map.containsKey pos map |> not

        let freePosition = 
            [(x, y+1); (x-1, y+1); (x+1, y+1)]
            |> Seq.filter isFree
            |> Seq.tryHead

        match freePosition with
        | None -> settleSand (500, 0) (Map.add (x,y) Sand map)
        | Some (_,y) when y > yMax -> map
        | Some pos -> settleSand pos map
    
    settleSand (500, 0) map

example
|> parse
|> dropSand
|> renderMap

// %%
input
|> parse
|> dropSand
//|> renderMap
|> Map.filter (fun k v -> v = Sand)
|> Map.count

// %%
let dropSand2 map =
    let yMax = (map |> Map.keys |> Seq.map snd |> Seq.max) + 1

    let rec settleSand (x, y) map = 
        let isFree pos = (Map.containsKey pos map |> not) && y <> yMax

        if Map.containsKey (500,0) map
        then map
        else 
            let freePosition = 
                [(x, y+1); (x-1, y+1); (x+1, y+1)]
                |> Seq.filter isFree
                |> Seq.tryHead

            match freePosition with
            | None -> settleSand (500, 0) (Map.add (x,y) Sand map)
            | Some (_,y) when y = 500 -> map
            | Some pos -> settleSand pos map
    
    settleSand (500, 0) map

example
|> parse
|> dropSand2
|> renderMap

// %%
input
|> parse
|> dropSand2
//|> renderMap
|> Map.filter (fun k v -> v = Sand)
|> Map.count

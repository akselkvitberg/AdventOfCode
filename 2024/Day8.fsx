// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 8
let example = GetExample 8 1
example

// %%
let parse input =
    let lines = input |> GetLines
    let antennas = 
        lines
        |> Seq.mapi (fun y l -> l |> Seq.mapi (fun x c -> (x,y), c))
        |> Seq.collect id
        |> Seq.filter (fun (_, c) -> c <> '.')
        |> Seq.groupBy snd
        |> Seq.map (fun (c, s) -> c, s |> Seq.map fst |> Seq.toList)
        |> Seq.toList
    
    let bounds = lines |> Seq.length, (lines |> Seq.head |> Seq.length)

    antennas, bounds
parse example

// %%
let getAntiNodes (width, height) (x1, y1) (x2, y2) =
    let dx = x2 - x1
    let dy = y2 - y1

    let checkBounds (x,y) = if x >= 0 && x < width && y >= 0 && y < height then Some (x,y) else None

    [
        checkBounds (x1-dx, y1-dy)
        checkBounds (x2+dx, y2+dy)
    ]
    |> List.choose id
    
getAntiNodes (12, 12) (4,3) (5,6)

// %%
let rec getPairs list = 
    [
        match list with 
        | h::t -> 
            for e in t do yield h, e
            yield! getPairs t
        | _ -> ()
    ]

[(1,2);(3,4);(5,6)] |> getPairs

// %%
let part1 input =
    let antennas, bounds = parse input
    printfn "%A" bounds

    [
        for antenna in antennas do
            let c, points = antenna
            let points = points |> getPairs

            points
            |> List.collect (fun (p1, p2) -> getAntiNodes bounds p1 p2)
    ]
    |> List.collect id
    |> List.distinct
    |> List.length  

part1 example

// %%
part1 input

// %%
let getAntiNodes2 (width, height) (x1, y1) (x2, y2) =
    let dx = x2 - x1
    let dy = y2 - y1

    let rec next (x,y) dx dy =
        [
            yield (x,y)
            let x' = x + dx
            let y' = y + dy
            if(x' < 0 || x' >= width || y' < 0 || y' >= height) 
            then ()
            else
                let nextPoint = (x', y')
                yield! next (x', y') (dx) (dy)
        ]
    
    if (dx = 0 && dy = 0) then []
    else 
        [
            yield! next (x1, y1) -dx -dy
            yield! next (x1, y1) dx dy
        ]
    
getAntiNodes2 (12, 12) (4,3) (5,6)

// %%
let part2 input =
    let antennas, bounds = parse input
    printfn "%A" bounds

    [
        for antenna in antennas do
            let c, points = antenna
            let points = points |> getPairs

            points
            |> List.collect (fun (p1, p2) -> getAntiNodes2 bounds p1 p2)
    ]
    |> List.collect id
    |> List.distinct
    |> List.length  

part2 example

// %%
part2 input

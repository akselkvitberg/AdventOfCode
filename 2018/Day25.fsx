#load "../utils.fsx"
open Utils

let input = GetData 25
let examples = [ for i in 1..4 -> GetExample 25 i ]
input

// %%
type Point = { X: int
               Y: int
               Z: int
               T: int }

let parseLine line =
    match Split "," line with
    | [|Int x; Int y; Int z; Int t|] -> {X = x
                                         Y = y
                                         Z = z
                                         T = t}
    
let parse input =
    GetLines input |> Array.map parseLine |> List.ofArray
    
parse input

// %%

let distance a b =
    abs (a.X - b.X) +
    abs (a.Y - b.Y) +
    abs (a.Z - b.Z) +
    abs (a.T - b.T)

let addPoint constellations point =
    let near, far =
        constellations
        |> List.partition (List.exists (fun p -> distance p point <= 3))
    (point :: List.concat near) :: far

let part1 input =
    let points = parse input
    
    points
    |> List.fold addPoint []
    |> List.length

part1 input
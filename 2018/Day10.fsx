#load "../utils.fsx"
open Utils

let input = GetData 10
let example = GetExample 10 1
example

// %%
type Vec2 = { X: int; Y: int }
type Point = { Pos: Vec2; Vel: Vec2 }

let parseLine =
    function
        | Regex @"position=<([\s-\d]+),([\s-\d]+)> velocity=<([\s-\d]+),([\s-\d]+)>" [Int x; Int y; Int vx; Int vy] ->
            {Pos=  {X=x; Y= y}; Vel = {X = vx; Y = vy}}
        | l -> failwith l
let parse input =
    input |> GetLines |> Array.map parseLine
    
parse example

// %%
let calculateBounds (points: Point array) =
    let minX, maxX = points |> Seq.map _.Pos.X |> Seq.min, points |> Seq.map _.Pos.X |> Seq.max
    let minY, maxY = points |> Seq.map _.Pos.Y |> Seq.min, points |> Seq.map _.Pos.Y |> Seq.max
    minX, minY, maxX, maxY
    
let visualize (points: Point array) =
    let minX, minY, maxX, maxY = calculateBounds points
    let lit = Array.map _.Pos points |> Set.ofArray
    [ for y in minY .. maxY ->
        System.String [| for x in minX .. maxX -> if lit.Contains {X = x; Y = y} then '#' else ' ' |] ]
    |> String.concat "\n"

parse example |> visualize 

// %%

let updatePoint (point:Point) = 
    { point with Pos = { X = point.Pos.X + point.Vel.X
                         Y = point.Pos.Y + point.Vel.Y }}
let step points =
    Array.map updatePoint points

// %%
let compare points =
    let _, minY, _, maxY = calculateBounds points
    maxY - minY

let findConverance input =
    input
    |> parse
    |> Seq.unfold (fun p -> Some (p, step p))
    |> Seq.pairwise
    |> Seq.indexed
    |> Seq.find (fun (_, (p1, p2)) -> compare p2 > compare p1)

let part1 input =
    input
    |> findConverance
    |> fun (_, (points, _)) -> visualize points

part1 example

// %%
part1 input

// %%
let part2 input =
    findConverance input
    |> fst
    

part2 example

// %%
part2 input

// %%

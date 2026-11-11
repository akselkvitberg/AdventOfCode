// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 9
let example = GetExample 9 1
example

// %%
type Point = int64 * int64
let parse lines =
    lines
    |> GetLines
    |> Array.map (function Regex "(\d+),(\d+)" [Long x; Long y] -> (x,y))

// %%
let getPairs (points: Point array) =
    [|
        for i in 0 .. points.Length - 1 do
            for j in i + 1 .. points.Length - 1 do
                yield (points[i], points[j])
    |]

// %%
let getArea ((x1,y1),(x2,y2)) =
    let width = (abs (x2 - x1)) + 1L
    let height = (abs (y2 - y1)) + 1L
    width * height 

// %%
let getLargestArea rectangles =
    rectangles
    |> Array.map getArea
    |> Array.max

// %%
let part1 input =
    input
    |> parse
    |> getPairs
    |> getLargestArea    
    
part1 example

// %%
part1 input

// Part 2 notes
// - Create a polygon from the points
// - Determine if a rectangle is fully inside the polygon
// - Calculate area of those rectangles and find the smallest
// %%
let createPolygon (points: Point array) =
    Array.pairwise points
    |> Array.append [| (points.[points.Length - 1], points.[0]) |]

// %%
type Bounds = { L: int64; R: int64; T: int64; B: int64 }

let rectangle ((x1,y1),(x2,y2)) =
    { L = (min x1 x2); R = (max x1 x2); T = (min y1 y2); B = (max y1 y2) }

let intersects rect edge =
    let a = rectangle rect
    let b = rectangle edge
    let left = a.L >= b.R
    let right = a.R <= b.L
    let above = a.B <= b.T
    let below = a.T >= b.B
    not (left || right || above || below)

// %%
let part2 input =
    let points = parse input
    let edges = createPolygon points
    let rectangles = getPairs points
    
    let intersectsAnyEdge rect = edges |> Array.tryFind(intersects rect) |> Option.isSome

    rectangles
    |> Seq.sortByDescending getArea
    |> Seq.find (intersectsAnyEdge >> not)
    |> getArea
    
part2 example

// %%
part2 input

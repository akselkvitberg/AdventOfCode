// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 18

// %%
let example = GetExample 18 1
example

// %%
type Instruction = {Direction: string; Distance: int64}

let parseSimple input =
    let parser instruction = 
        match instruction with
        | Regex "(.) (\d+)" [direction; Int distance] -> {Direction = direction; Distance = distance}
        | _ -> failwith "Bad input"
        
    input |> GetLines |> Array.map parser

parseSimple example

// %%
let (|Hex|_|) (str:string) = Some (System.Convert.ToInt64(str, 16))

let parseAdvanced input =
    let parser instruction =
        match instruction with
        | Regex ". \d+ \(#(.{5})(.)\)" [Hex distance; direction] -> {Direction = direction; Distance = distance}
        | _ -> failwith $"Bad input"

    input |> GetLines |> Array.map parser

parseAdvanced example

// %%
let getVector = 
    function 
    | "0" | "R" -> ( 1L,  0L)
    | "2" | "L" -> (-1L,  0L)
    | "1" | "D" -> ( 0L,  1L)
    | "3" | "U" -> ( 0L, -1L)
    | _ -> failwith ("Unknown direction")

let getNextVertex (x, y) input = 
    let (dx,dy) = getVector input.Direction
    (x + (dx * input.Distance), y + (dy * input.Distance))

getNextVertex (0,0) {Distance = 3; Direction = "R"}

// %%
let getPerimeter = Array.scan getNextVertex (0,0)

example |> parseSimple |> getPerimeter 

// %%
let getBorder = Array.sumBy (_.Distance)

let getRectArea ((x1, y1),(x2, y2)) = (y1 - y2) * (x1 + x2)

let getEnclosingArea vertices = 
    vertices
    |> Array.pairwise
    |> Array.map getRectArea
    |> Array.sum

example |> parseSimple |> getPerimeter |> getEnclosingArea

// %%
let getArea parser input =
    let instructions = parser input
    let area = getPerimeter instructions |> getEnclosingArea
    let border = getBorder instructions
    // picks theorem
    border / 2L + abs (area) / 2L + 1L

getArea parseSimple example

// %%
getArea parseSimple input

// %%
getArea parseAdvanced example

// %%
getArea parseAdvanced input

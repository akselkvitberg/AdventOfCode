#load "../utils.fsx"
open Utils

let input = GetData 21
let example = """../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#"""
example

// %% types
type Grid = char array array // [row][col]
type Block = char array array // [row][col]
type Rulebook = Map<Block, Block>

// %%
let parsePattern pattern =
    Split "/" pattern |> Array.map Array.ofSeq
//%%
let startingImage = parsePattern """.#./..#/###"""
startingImage
// %%
let parseLine input =
    match input with
    | Regex "(.+) => (.+)" [a;b] -> parsePattern a, parsePattern b
    | line -> failwith line

let parse input =
    GetLines input |> Array.map parseLine

parse example

// %%
let flipPattern pattern = pattern |> Array.map Array.rev
// %%
let rotatePattern pattern = pattern |> Array.transpose |> flipPattern
//%%
let patternVariations pattern =
    [|
        pattern
        pattern |> rotatePattern
        pattern |> rotatePattern |> rotatePattern
        pattern |> rotatePattern |> rotatePattern |> rotatePattern
    |]
    |> Array.collect (fun p -> [|p; flipPattern p|])
// %%
let rulebook input : Rulebook =
    let patterns = parse input
    patterns
    |> Array.collect (fun (from, output) -> patternVariations from |> Array.map (fun v -> v, output))
    |> Map.ofArray
    
//rulebook input

// %%
let fromBlocks (blocks: Block array array) : Grid =
    blocks
    |> Array.collect (fun blockRow -> Array.init blockRow[0].Length (fun i -> blockRow |> Array.collect (fun b ->b[i])))

let toBlocks (grid: Grid) : Block array array =
    let n = if grid.Length % 2 = 0 then 2 else 3    
    grid
    |> Array.chunkBySize n
    |> Array.map (fun band ->
        [| for c in 0..n..grid.Length - 1 ->
            band |> Array.map (fun row -> row.[c..c+n-1]) |])


// %%
let enhanceGrid (rules:Rulebook) (grid:Grid) =
    grid
    |> toBlocks
    |> Array.map (Array.map (fun block -> Map.find block rules))
    |> fromBlocks

// %%
let solve iterations input =
    let rules = rulebook input
    [1..iterations]
    |> List.fold (fun p _ -> enhanceGrid rules p) startingImage
    |> Array.collect id
    |> Array.filter ((=) '#')
    |> Array.length
    
solve 2 example

// %%
solve 5 input

// %%
solve 18 input
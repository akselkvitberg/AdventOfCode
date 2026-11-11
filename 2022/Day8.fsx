// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 8 |> GetLines

input[0].Length, input.Length

// %%
let example = [
    "30373"
    "25512"
    "65332"
    "33549"
    "35390"
]

// %%
let parse (input:string) =
    input
    |> Seq.map (Char.GetNumericValue >> int)

let parseInput input =
    input
    |> Seq.map parse
    |> array2D

let exampleForest = parseInput example
let forest = parseInput input
printf "%A" exampleForest

// %%
printfn "%A" (exampleForest[*,0]) // column
printfn "%A" (exampleForest[0,*]) // row

// %%
let getIndexedRow (array:int[,]) row =
    array[row,*]
    |> Seq.mapi (fun i v -> (row, i), v)

let getIndexedColumn (array:int[,]) column =
    array[*,column]
    |> Seq.mapi (fun i v -> (i, column), v)

// %%
let countVisibleTreesAlong line =
    line
    |> Seq.fold (fun (trees, height) (pos, v) -> if v > height then (pos::trees, v) else (trees, height)) ([], -1)
    |> fst

getIndexedColumn exampleForest 1
|> countVisibleTreesAlong

// %%
let countVisibleTrees array =
    let size = Array2D.length1 array
    seq {
        for i in [0..size-1] do
            let row = getIndexedRow array i
            let col = getIndexedColumn array i
            yield countVisibleTreesAlong row
            yield countVisibleTreesAlong (row |> Seq.rev) 
            yield countVisibleTreesAlong col
            yield countVisibleTreesAlong (col |> Seq.rev)
    }
    |> Seq.collect id
    |> Seq.distinct
    |> Seq.length

countVisibleTrees exampleForest

// %%
// Part 1
countVisibleTrees forest

// %%
let countVisibleTreesAlongSimple height line =
    seq {
        yield! line |> Seq.takeWhile (fun v -> v < height)
        yield! line |> Seq.skipWhile (fun v -> v < height) |> Seq.truncate 1
    }
    |> Seq.length

countVisibleTreesAlongSimple 5 [4;2;2;3;4;3;6]

// %%
let countVisibleTreesFrom (array:int[,]) (x,y) =
    let size = Array2D.length1 array
    let height = array[y,x]
    let left = array[y,0..x-1]      |> Seq.rev |> countVisibleTreesAlongSimple height
    let right = array[y, x+1..size] |> countVisibleTreesAlongSimple height
    let up = array[0..y-1, x]       |> Seq.rev |> countVisibleTreesAlongSimple height
    let down = array[y+1..size, x]  |> countVisibleTreesAlongSimple height

    up * left * right * down

countVisibleTreesFrom exampleForest (2,1),
countVisibleTreesFrom exampleForest (2,3),
countVisibleTreesFrom forest (49, 49)

// %%
let getPositionsToConsider array =
    let size = Array2D.length1 array
    seq {
        for x in [1..size-2] do
        for y in [1..size-2] do
        (x,y)
    }

getPositionsToConsider exampleForest
|> Seq.map (countVisibleTreesFrom exampleForest)
|> Seq.max

// %%
getPositionsToConsider forest
|> Seq.map (countVisibleTreesFrom forest)
|> Seq.max

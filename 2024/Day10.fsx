// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 10
let example = GetExample 10 5
example

// %%
let parse input = 
    input
    |> GetLines
    |> Array.map (Seq.map charToInt)
    |> array2D

parse example

// %%
let findTrailHeads (array: int [,]) =
    [
        for y in 0..array.GetLength(1)-1 do
        for x in 0..array.GetLength(0)-1 do
        if array[x, y] = 0 then yield (x,y)
    ]

findTrailHeads (parse example)

// %%
let dfs (array: int [,]) (startX,startY) =
    let rec loop (x,y) previousValue =
        seq {
            if(x < 0 || y < 0 || x >= array.GetLength(0) || y >= array.GetLength(1) || array[x,y] <> previousValue + 1) then ()
            elif previousValue = 8 then yield (x,y)
            else 
                yield! loop (x+1,y) (previousValue + 1)
                yield! loop (x-1,y) (previousValue + 1)
                yield! loop (x,y+1) (previousValue + 1)
                yield! loop (x,y-1) (previousValue + 1)
        }

    loop (startX,startY) -1

dfs (parse example) (6,0)

// %%
let part1 input =
    let array = parse input
    findTrailHeads array
    |> List.map (dfs array >> Seq.distinct >> Seq.length)
    |> List.sum

part1 example

// %%
part1 input

// %%
let part2 input =
    let array = parse input
    findTrailHeads array
    |> List.map (dfs array)
    |> Seq.collect id
    |> Seq.length

part2 example

// %%
part2 input

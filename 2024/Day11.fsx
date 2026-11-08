// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 11
let example = "0 1 10 99 999"
example

// %%
let parse input = input |> Split " " |> Array.map int64 |> List.ofArray

parse example

// %%
let (|SplitStone|_|) (number:int64) =
    let number = string number

    if number.Length % 2 = 0 then
        Some [
            int64 number[..number.Length/2-1]
            int64 number[number.Length/2..]
        ]
    else
        None

match 123456789L with 
| SplitStone data -> printfn "%A" data
| _ -> printfn "None"

match 12345678L with 
| SplitStone data -> printfn "%A" data
| _ -> printfn "None"

// %%
let nextStones (stone: int64) =
    match stone with
    | 0L -> [1L]
    | SplitStone data -> data
    | _ -> [stone * 2024L]

// %%
let step (stones: int64 list) = 
    stones |> List.collect nextStones

parse example |> step

// %%
let part1 input =
    let stones = parse input

    [1..25]
    |> List.fold (fun acc _ -> step acc) stones
    |> List.length

part1 "125 17"

// %%
part1 input

// Part 2 cannot be solved the naive way, we need to optimize.
// %%
let runStepPart2 (stoneGroup: (int64 * int64) list) =
    stoneGroup
    |> List.collect (fun (stone, count) -> (nextStones stone |> List.map (fun x -> x, count)))
    |> List.groupBy fst
    |> List.map (fun (x, y) -> x, List.sumBy snd y)

let part2 input = 
    let stones = parse input |> List.countBy id |> List.map (fun (x, y) -> x, int64 y)
    
    [1..75]
    |> List.fold (fun acc _ -> runStepPart2 acc) stones
    |> List.sumBy snd

part2 "125 17"

// %%
part2 input

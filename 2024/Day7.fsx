// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 7
let example = GetExample 7 1
example

// %%
let parse input = 
    GetLines input
    |> Array.map (fun line -> line.Split([|':'; ' '|], StringSplitOptions.RemoveEmptyEntries) |> Array.map int64 |> fun a -> a[0], a[1..] |> List.ofArray)

parse example

// %%
let rec canSolve operators result acc values = 
    match values with 
    | [] -> acc = result
    | a::rest -> 
        List.exists (fun op -> canSolve operators result (op acc a) rest) operators

canSolve [(+); (*)] 10L 1L [2L; 3L; 4L],
canSolve [(+); (*)] 15L 3L [5L],
canSolve [(+); (*)] 14L 3L [5L]

// %%
let part1 input =
    parse input
    |> Array.filter (fun (result, x::values) -> canSolve [(+); (*)] result x values)
    |> Array.sumBy (fun (result, _) -> result)

part1 example

// %%
part1 input

// %%
let part2 input =
    parse input
    |> Array.filter (fun (result, x::values) -> canSolve [(+); (*); (fun a b -> int64 $"{a}{b}")] result x values)
    |> Array.sumBy (fun (result, _) -> result)

part2 example

// %%
part2 input

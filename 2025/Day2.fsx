// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 2
let example = GetExample 2 1 |> fun x -> x.ReplaceLineEndings("") // example is split into lines for readability - remove line endings
example

// %%
let expand input =
    input
    |> Split ","
    |> Array.collect (Split "-" >> Array.map int64 >> fun [| a; b |] -> [| for x in a .. b -> x |])

expand example

// %%
let isInvalidId number =
    let str = string number
    match str with
    | Regex @"^(.+)\1$" [_] -> true
    | _ -> false

isInvalidId 1212

// %%
let part1 = expand >> Array.filter isInvalidId >> Array.sum
part1 example

// %%
part1 input

// %%
let isInvalidId2 number =
    let str = string number
    match str with
    | Regex @"^(.+)\1+$" [_] -> true
    | _ -> false


isInvalidId2 824824824

// %%
let part2 = expand >> Array.filter isInvalidId2 >> Array.sum
part2 example

// %%
part2 input

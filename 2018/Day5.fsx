// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 5 |> Seq.toList
let examples = [
    "dabAcCaCBAcCcaDA"
    "dabAaCBAcCcaDA"
    "dabCBAcCcaDA"
    "dabCBAcaDA"
]
input

// %%
let rec react output input =
    match input with
    | x :: y :: rest when abs (int x - int y) = 32 ->
        match output with
        | prev :: output' -> react output' (prev :: rest)
        | [] -> react output rest
    | x :: rest -> react (x::output) rest
    | [] -> output

react [] input

// %%
let getResultingLength input =
    react [] (input |> Seq.toList) |> List.length

getResultingLength input

// %%
let getProblematicUnit input =
    ['a' .. 'z']
    |> List.map (fun c ->
        input
        |> List.filter (fun x -> Char.ToLower x <> c)
        |> getResultingLength)
    |> List.min

getProblematicUnit input

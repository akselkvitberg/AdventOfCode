// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 1
input[0..10]

// %%
let direction c =
    match c with
    | '(' -> 1
    | ')' -> -1
    | _ -> 0

// %%
input |> Seq.map direction |> Seq.sum

// %%
input |> Seq.map direction |> Seq.scan (+) 0 |> Seq.findIndex (fun x -> x < 0)

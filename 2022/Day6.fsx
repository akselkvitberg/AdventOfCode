// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 6
input[..20], input.Length

// %%
let findUniqueCharacters count list =
    list
    |> Seq.windowed count
    |> Seq.map Array.distinct
    |> Seq.findIndex (fun x -> x.Length = count)
    |> (+) count

findUniqueCharacters 4 input

// %%
findUniqueCharacters 14 input

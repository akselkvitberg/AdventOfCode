// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 1

let elves = 
    input
    |> Split "\n\n"
    |> Array.map (GetLines >> Array.map int)

elves[..4]

// %%
elves
|> Seq.map (Seq.sum)
|> Seq.max

// %%
elves
|> Seq.map (Seq.sum)
|> Seq.sortByDescending id
|> Seq.take 3
|> Seq.sum

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let lines = GetData 1 |> GetLines
let items = lines |> Seq.map int

// %%
let increases (a,b) = a < b
items |> Seq.pairwise |> Seq.filter increases |> Seq.length

// %%
items |> Seq.windowed 3 |> Seq.map Seq.sum |> Seq.pairwise |> Seq.filter increases |> Seq.length

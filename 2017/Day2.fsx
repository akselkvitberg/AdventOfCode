// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 2
input

// %%
let parseLine = Split "\t" >> Array.map int

let parse input =
    input
    |> GetLines
    |> Array.map parseLine

parse input

// %%
let checksum row =
    let min = row |> Seq.min
    let max = row |> Seq.max
    max - min

input
|> parse
|> Seq.sumBy checksum

// %%
let getEvenlyDivisable row =
    Seq.allPairs row row
    |> Seq.where(fun (a,b) -> a <> b && a % b = 0)
    |> Seq.sumBy (fun (a,b) -> a / b)

input
|> parse
|> Seq.sumBy getEvenlyDivisable

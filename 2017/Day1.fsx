// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 1
input

// %%
input
|> Seq.map charToInt
|> Seq.pairwise
|> Seq.where (fun (a,b) -> a = b )
|> Seq.sumBy fst

// %%
let numbers =
    input.ToCharArray()
    |> Array.map charToInt
let half = numbers.Length / 2

numbers
|> Seq.zip (Seq.append numbers[half..] numbers[0..half])
|> Seq.where (fun (a,b) -> a = b )
|> Seq.sumBy fst

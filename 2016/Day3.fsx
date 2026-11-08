// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 3 |> GetLines |> Array.map (Split " " >> Array.map int)
input[..4]

// %%
let isValid [|a;b;c|] = a + b > c && b + c > a && c + a > b

isValid [|5;10;25|]

// %%
input |> Seq.countBy isValid

// %%
let rotate (a:'T[][]) = 
    [| for x in {0 .. a.[0].Length-1} -> [| for y in {0 .. a.Length-1} -> a.[y].[x] |] |]

// %%
input
|> Seq.chunkBySize 3
|> Seq.collect rotate
|> Seq.countBy isValid

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 17 |> GetLines |> Seq.map int |> Seq.toList

// %%
let rec Combine (sizes:int list) =
    seq {
            match sizes with
            | [] -> yield []
            | [v] -> yield [v]
            | (v::tail) ->
                yield [v]
                for r in Combine tail do
                    yield v::r
                    yield r
        }

let combinations = Combine input |> Seq.toList 
combinations |> Seq.length

// %%
combinations 
|> Seq.map Seq.sum 
|> Seq.filter (fun x -> x = 150)
|> Seq.length

// %%
combinations
|> Seq.filter (fun x -> Seq.sum x = 150)
|> Seq.countBy (fun x-> x.Length)
|> Seq.sort
|> Seq.head
|> snd

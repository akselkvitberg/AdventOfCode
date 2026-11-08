// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 1 |> GetLines |> Seq.map int
input

// %%
input |> Seq.sum

// %%
open System.Collections.Generic

let cycle items = 
    seq {
        while true do
            yield! items
    }

let findFirstDuplicateInfiniteSeq seq =
    let seen = HashSet<'T>()
    seq
    |> Seq.pick (fun x ->
        if seen.Contains(x) then Some x
        else
            seen.Add(x) |> ignore
            None
    )

cycle input
|> Seq.scan (+) 0
|> findFirstDuplicateInfiniteSeq

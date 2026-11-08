// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#r "nuget: FSharp.Data, 5.0.2"
open FSharp.Data

// %%
#load "../utils.fsx"
open Utils
let input = GetData 12

// %%
let matches = RegularExpressions.Regex.Matches(input, "(-?\d+)")
matches |> Seq.map (fun m -> m.Captures[0].Value) |> Seq.map int |> Seq.sum

// %%
let isRed record = 
    record 
    |> Seq.map snd 
    |> Seq.exists (fun v ->
        match v with
        | JsonValue.String s when s = "red" -> true
        | _ -> false )

let rec sumNumbers (json:JsonValue) = 
    match json with
    | JsonValue.Number value -> int value
    | JsonValue.Record record when not (isRed record) -> record |> Seq.sumBy (fun (_, v) -> sumNumbers v)
    | JsonValue.Array arr -> arr |> Seq.sumBy sumNumbers
    | _ -> 0

// %%
JsonValue.Parse input |> sumNumbers

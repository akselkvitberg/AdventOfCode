// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 6
let example = GetExample 6 1
example

// %%
let parseOp = 
    function
    | "*" -> (*)
    | "+" -> (+)

let calculateColumn (items:string array) =
    let variables = items.[0..^1] |> Array.map int64
    let op = parseOp items.[^0]
    Array.reduce (fun acc i -> op acc i) variables

let part1 input =
    input
    |> GetLines
    |> Array.map (Split " ")
    |> Array.transpose
    |> Array.map calculateColumn
    |> Array.sum
part1 example

// %%
part1 input

// %%
let split input =
    let result = ResizeArray()
    let buffer = ResizeArray()
    for item in input do
        match item with
        | None when buffer.Count = 0 -> ()
        | None ->
            result.Add(buffer.ToArray())
            buffer.Clear()
        | Some x -> buffer.Add(x)
    if buffer.Count > 0 then
        result.Add(buffer.ToArray())
    result.ToArray()

// %%
let tryParseInt64 (s: string) =
    match Int64.TryParse(s) with
    | true, v -> Some v
    | _ -> None

let part2 (input: string) =
    let lines = input.Split("\n")
    let operators = lines[^0] |> Split " " |> Array.map parseOp
    lines[0..^1]
    |> Array.map (Array.ofSeq)
    |> Array.transpose
    |> Array.map (System.String >> tryParseInt64)
    |> split
    |> Array.mapi (fun i values ->
        Array.reduce (operators[i]) values
    )
    |> Array.sum
part2 example

// %%
part2 input

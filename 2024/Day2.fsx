// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 2 |> GetLines
let example = GetExample 2 1 |> GetLines
example

// %%
let parse = Array.map (Split " " >> Array.map int)
example |> parse

// %%
let isSafe report =
    let direction = 
        report 
        |> Seq.pairwise
        |> Seq.map (fun (a,b) -> a - b)
        |> Set.ofSeq
    Set.isSubset direction (Set.ofList [1;2;3]) || Set.isSubset direction (Set.ofList [-1;-2;-3])

parse example |> Array.map isSafe

// %%
parse input |> Array.filter isSafe |> Array.length

// %%
let safeWithProblemDampner (report:int array) =
    if isSafe report then true
    else 
        [0..report.Length-1]
        |> List.exists (fun i -> report |> Seq.removeAt i |> isSafe)

parse example |> Array.map safeWithProblemDampner

// %%
parse input |> Array.filter safeWithProblemDampner |> Array.length

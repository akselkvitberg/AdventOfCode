// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 11
let example = GetExample 11 1
let example2 = GetExample 11 2
example

// %%
let parseLine (line:string) =
    let [|node; nodes |] = line |> Split ":"
    let items = nodes |> Split " "
    node, items

let parse input =
    input 
    |> GetLines
    |> Array.map parseLine
    |> Map.ofArray

example |> parse

// %%
let findAllPaths start dest (map: Map<string, string array>) =
    
    let rec aux current =
        [
            if current = dest then yield [dest]
            else
                let outgoingNodes = map.[current]
                for node in outgoingNodes do
                    let subPaths = aux node
                    for path in subPaths do
                        yield current :: path
        ]
    aux start
    |> Seq.map Set.ofList
    |> Array.ofSeq

let part1 input = 
    input 
    |> parse
    |> findAllPaths "you" "out"
    |> Array.length

// %%
part1 input

// %%
let part2 input =
    let passThrough = Set.ofList ["dac"; "fft"]
    input
    |> parse
    |> findAllPaths "svr" "out"
    |> Array.filter (fun path -> path |> Set.intersect passThrough |> Set.count = 2)

part2 example2

// %%
// too slow
//part2 input

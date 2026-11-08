// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 14

// %%
let example = GetExample 14 1
example

// %%
type Cell = Cube | Rock | Empty

let parseChar = 
    function
    | 'O' -> Rock
    | '#' -> Cube
    | '.' -> Empty
    | _ -> failwith "bad input"

let parse input =
    input |> GetLines |> Array.map (Seq.map parseChar >> Seq.toList) |> Array.toList

parse example

// %%
let drawPlatform (platform: Cell list list) =
    platform
    |> List.map (List.map (function Rock -> 'O' | Cube -> '#' | _ -> '.') >> List.toArray >> String)
    |> String.concat "\n"
    |> printfn "%s\n"
    platform

example |> parse |> drawPlatform |> ignore

// %%
type Direction = North | South | East | West
type Step = Forwards | Backwards

let rotate direction step =
    match direction, step with
    | North, _ -> List.transpose
    | South, Forwards -> List.rev >> List.transpose
    | South, Backwards -> List.transpose >> List.rev
    | East, _ -> List.map List.rev
    | West, _ -> id

example |> parse |> rotate North Forwards  |> rotate North Backwards |> drawPlatform |> ignore

// %%
// test method to ensure rotate function is implemented correctly. Rotation forwards and backwards should be the identity function
let ensureRotationIsIdentity input = 
    let platform = parse input

    let rotateTest direction = rotate direction Forwards >> rotate direction Backwards

    [
        platform |> rotateTest North
        platform |> rotateTest South
        platform |> rotateTest East
        platform |> rotateTest West
    ]
    |> List.forall (fun rotation -> platform = rotation)
    
ensureRotationIsIdentity example

// %%
let splitRow (lst: Cell list) =
    let rec splitIntern (acc:Cell list) (remaining: Cell list) =
        [
            match acc, remaining with
            | _, [] -> yield acc |> List.rev
            | _, x::xs when x = Cube -> 
                yield acc |> List.rev
                yield! splitIntern [] xs
            | _, x::xs -> yield! splitIntern (x::acc) xs
        ]
    splitIntern [] lst

splitRow [Cube; Empty; Empty; Rock; Cube; Empty; Rock; Empty; Cube; Rock; Empty; Empty; Cube; ]

// %%
let combineRow list = List.foldBack (fun x -> function | [] -> [x] | xs -> x::[Cube]::xs) list [] |> List.collect id
splitRow [Cube; Empty; Empty; Rock; Cube; Empty; Rock; Empty; Cube; Rock; Empty; Empty; Cube; ] |> combineRow

// %%
let tiltRow = splitRow >> List.map List.sort >> combineRow

tiltRow [Cube; Empty; Empty; Rock; Cube; Empty; Rock; Empty; Cube; Rock; Empty; Empty; Cube; ]

// %%
let tiltPlatform = List.map tiltRow

let tilt direction platform = 
    platform
    |> rotate direction Forwards
    |> tiltPlatform
    |> rotate direction Backwards

parse example |> drawPlatform |> tilt North |> drawPlatform |> ignore

// %%
let calculateWeights (platform: Cell list list) =
    platform
    |> rotate South Forwards
    |> List.map (List.mapi (fun row -> function | Rock -> row+1 | _ -> 0))
    |> List.sumBy (List.sum)

parse example |> tilt North |> calculateWeights

// %%
let part1 input =
    parse input
    |> tilt North
    |> calculateWeights

part1 input

// %%
let doCycle = 
    [ North; West; South; East ]
    |> List.map tilt |> List.fold (>>) id

example |> parse |> Seq.unfold (fun state -> Some (state, doCycle state)) |> Seq.take 10 |> Seq.last |> (drawPlatform) |> ignore

// %%
open System.Collections.Generic

let getKey platform =
    platform

let detectCycle platform =
    let dictionary = Dictionary<_, int>()

    let rec run state iteration =
        let key = getKey state
        if dictionary.ContainsKey(key) 
        then 
            dictionary[key], iteration, dictionary |> Seq.map (fun (k) -> k.Value, calculateWeights k.Key) |> dict
        else
            dictionary.Add(key, iteration)
            run (doCycle state) (iteration + 1)

    run platform 0

example |> parse |> detectCycle

// %%
let runCycles input iterations =
    let (startCycle, endCycle, weights) = input |> parse |> detectCycle
    let cycle = endCycle - startCycle
    let cycleIndex = (iterations-startCycle) % cycle
    cycle, cycleIndex, weights[cycleIndex + startCycle]

runCycles example 1000000000

// %%
runCycles input 1000000000

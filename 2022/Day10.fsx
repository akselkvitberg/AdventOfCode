// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 10 |> GetLines
input[..5]

// %%
let example = File.ReadAllLines "Inputs/10-example.txt" // large example from instructions
example[..5]

// %%
type Instruction = 
    | Noop 
    | AddX of int 

// %%
let parse (input:string) = 
    match input.Split(" ") with
    | [|"noop"|] -> [|Noop|]
    | [|"addx" ; Int v|] -> [|Noop; AddX v|] // Addx takes two cycles

["noop"; "addx 4"; "addx -1"] |> Seq.collect parse

// %%
let processInstructions (register, cycle) instruction =
    match instruction with
    | Noop -> (register, cycle + 1)
    | AddX v -> (register + v, cycle + 1)

[Noop; AddX 4] |> Seq.scan processInstructions (0, 0)

// %%
let getSignalStrength input =
    input
    |> Seq.collect parse
    |> Seq.scan processInstructions (1,1)
    |> (Seq.skip 19 >> Seq.chunkBySize 40 >> Seq.map Seq.head) // Get every 40'th element starting at element 20
    |> Seq.sumBy (fun (a,b) -> a*b)

getSignalStrength example

// %%
getSignalStrength input

// %%
let drawPixel (pos, scan) =
    if abs (pos - (scan % 40)) <= 1 
    then '█'
    else ' '

let drawScreen input =
    input
    |> Seq.collect parse
    |> Seq.scan processInstructions (1,0)
    |> Seq.map drawPixel
    |> Seq.chunkBySize 40
    |> Seq.map String
    |> String.concat "\n"

drawScreen example

// %%
drawScreen input

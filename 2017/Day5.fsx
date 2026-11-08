// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 5 |> GetLines |> Seq.map int |> Seq.indexed |> Map
input

// %%
let rec processInstruction incrementFunction instructions pc steps =
    if (pc >= (instructions |> Map.count))
        then steps
    else
        let instruction = instructions |> Map.find pc
        processInstruction incrementFunction (instructions |> Map.add pc (incrementFunction instruction)) (pc + instruction) (steps + 1L)

let add1 = (+) 1
processInstruction add1 input 0 0L

// %%
let addOrSubtract1 offset =
    if (offset >= 3) then offset - 1 else  offset + 1

processInstruction addOrSubtract1 input 0 0L

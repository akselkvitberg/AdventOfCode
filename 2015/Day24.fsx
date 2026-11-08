// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = 
    GetData 24
    |> Seq.map uint64 |> Seq.toList

let QuantumEntanglement (list:uint64 list) = list |> Seq.reduce (*)

// %%
let rec GetCombinations groupWeight total group1 (list:uint64 list) = 
    seq {
        match list with
        | [a] when total + a = groupWeight -> yield a::group1 
        | a::tail ->
            if total + a < groupWeight 
            then yield! GetCombinations groupWeight (total + a) (a::group1) tail
            else ()
            yield! GetCombinations groupWeight total group1 tail
        | _ when total = groupWeight -> yield group1
        | _ -> ()
    }

// %%
let GetQuantumBalance compartments =
    let groupWeight = (input |> Seq.sum) / uint64 compartments

    GetCombinations groupWeight 0UL [] input 
    |> Seq.cache
    |> Seq.sortBy (fun x -> QuantumEntanglement x, Seq.length x)
    |> Seq.map QuantumEntanglement
    |> Seq.head

GetQuantumBalance 3

// %%
GetQuantumBalance 4

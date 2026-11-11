// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 3 |> GetLines
let example = GetExample 3 1 |> GetLines
example

// %%
let getHighestConsecutiveNumbers digits (input:string) =
    let numbers = input |> Seq.map charToInt64 |> Seq.toList
    let rec findHighestDigitIn digit list = 
        if digit = 0 then 0L
        else
            let ix, value = list |> Seq.indexed |> Seq.truncate (List.length list - digit+1) |> Seq.maxBy snd
            let v = value * pown 10L (digit - 1)
            let arr = List.skip (ix+1) list
            v + findHighestDigitIn (digit - 1) arr
    
    findHighestDigitIn digits numbers
    
getHighestConsecutiveNumbers 2 example[2]

// %%
let sumAllHighestConsecutiveNumbers digits lines =
    lines
    |> Seq.map (getHighestConsecutiveNumbers digits)
    |> Seq.sum
sumAllHighestConsecutiveNumbers 2 example

// %%
sumAllHighestConsecutiveNumbers 2 input

// %%
sumAllHighestConsecutiveNumbers 12 input

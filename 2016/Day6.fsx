// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 6 |> GetLines
input[..5]

// %%
let testInput = [|
    "eedadn"
    "drvtee"
    "eandsr"
    "raavrd"
    "atevrs"
    "tsrnev"
    "sdttsa"
    "rasrtv"
    "nssdts"
    "ntnada"
    "svetve"
    "tesnvt"
    "vntsnd"
    "vrdear"
    "dvrsen"
    "enarar"
|]

// %%
let rotate (a:'T[][]) = 
    [| for x in {0 .. a.[0].Length-1} -> [| for y in {0 .. a.Length-1} -> a.[y].[x] |] |]

let solve1 input =
    input
    |> Array.map Seq.toArray
    |> rotate
    |> Seq.map (Seq.countBy id >> Seq.sortByDescending snd >> Seq.head)
    |> Seq.map fst
    |> Seq.toArray
    |> fun ch -> new string(ch)

solve1 testInput

// %%
solve1 input

// %%
let solve2 input =
    input
    |> Array.map Seq.toArray
    |> rotate
    |> Seq.map (Seq.countBy id >> Seq.sortBy snd >> Seq.head)
    |> Seq.map fst
    |> Seq.toArray
    |> fun ch -> new string(ch)

solve2 testInput

// %%
solve2 input

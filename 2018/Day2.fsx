// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 2 |> GetLines
input

// %%
let hasTwoOfAnyLetter (s:string) =
    s |> Seq.groupBy id |> Seq.exists (fun (_, g) -> Seq.length g = 2)
let hasThreeOfAnyLetter (s:string) =
    s |> Seq.groupBy id |> Seq.exists (fun (_, g) -> Seq.length g = 3)

let twoCount = input |> Seq.filter hasTwoOfAnyLetter |> Seq.length
let threeCount = input |> Seq.filter hasThreeOfAnyLetter |> Seq.length
twoCount * threeCount

// %%
let getMatchingBoxes (a:string, b:string) =
    Seq.zip a b
    |> Seq.toArray
    |> Array.partition (fun (x, y) -> x = y)
    |> function 
         | (matches, [|one|]) ->
              matches
              |> Array.map fst
              |> String
              |> Some
         | _ -> None

let getCombinations (boxes: string list) = 
    seq { 
        for i in 0 .. boxes.Length - 1 do
        for j in i + 1 .. boxes.Length - 1 do
            yield (boxes.[i], boxes.[j]) 
    }
    |> Seq.pick getMatchingBoxes

getCombinations (input |> Seq.toList)

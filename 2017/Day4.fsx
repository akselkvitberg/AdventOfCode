// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 4 |> GetLines
input[0..4]

// %%
let checkPassword wordProcessor password =
    password 
    |> Split " "
    |> Seq.countBy wordProcessor
    |> Seq.forall (fun (_,count) -> count = 1)

checkPassword id input[0], checkPassword id input[4]

// %%
input
|> Seq.where (checkPassword id)
|> Seq.length

// %%
let sortLetters (word:string) =
    word |> Seq.sort |> Seq.toArray

input
|> Seq.where (checkPassword sortLetters)
|> Seq.length

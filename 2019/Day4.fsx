// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 4 |> Split "-" |> Array.map int
input

// %%
let matchPasswordRules (password:string) = 
    let hasTwoAdjacentDigits  = password |> Seq.pairwise |> Seq.exists (fun (x,y) -> x = y)
    let allDigitsIncreasing = password |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <= y)
    hasTwoAdjacentDigits && allDigitsIncreasing 

matchPasswordRules "112340"

// %%
let getAllMatchingPasswords rangeStart rangeStop =
    [rangeStart..rangeStop]
    |> Seq.map string
    |> Seq.where matchPasswordRules

getAllMatchingPasswords input[0] input[1]
|> Seq.length

// %%
let hasExactlyTwoAdjecentDigits (pwd:string) = 
    let rec processDigit last count (rest:char list) =
        match rest with 
        | [] when count = 2 -> true
        | [] -> false
        | x::xs when x <> last && count = 2 -> true
        | x::xs when x <> last -> processDigit x 1 xs
        | x::xs -> processDigit x (count + 1) xs
    processDigit ' ' 0 (pwd |> Seq.toList)

hasExactlyTwoAdjecentDigits "111122"

// %%
let matchPasswordRules2 (password:string) = 
    let hasTwoAdjacentDigits  = password |> hasExactlyTwoAdjecentDigits
    let allDigitsIncreasing = password |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <= y)
    hasTwoAdjacentDigits && allDigitsIncreasing 

matchPasswordRules2 "111234"

// %%
let getAllMatchingPasswords rangeStart rangeStop =
    [rangeStart..rangeStop]
    |> Seq.map string
    |> Seq.where matchPasswordRules2

getAllMatchingPasswords input[0] input[1]
|> Seq.length

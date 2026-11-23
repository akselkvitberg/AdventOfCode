#load "../utils.fsx"
open Utils

let input = GetData 4 |> Split "-" |> Array.map int
input

// %%
let allDigitsIncreasing (password: string) =
    password |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <= y)

let matchPasswordRules (password:string) = 
    let hasTwoAdjacentDigits  = password |> Seq.pairwise |> Seq.exists (fun (x,y) -> x = y)
    hasTwoAdjacentDigits && allDigitsIncreasing password

matchPasswordRules "112340"

// %%
let getAllMatchingPasswords rule rangeStart rangeStop =
    seq { rangeStart..rangeStop }
    |> Seq.map string
    |> Seq.filter rule

getAllMatchingPasswords matchPasswordRules input[0] input[1]
|> Seq.length

// %%
let hasExactlyTwoAdjacentDigits (pwd:string) = 
    let rec processDigit last count (rest:char list) =
        match rest with 
        | [] when count = 2 -> true
        | [] -> false
        | x::xs when x <> last && count = 2 -> true
        | x::xs when x <> last -> processDigit x 1 xs
        | x::xs -> processDigit x (count + 1) xs
    processDigit ' ' 0 (pwd |> Seq.toList)

hasExactlyTwoAdjacentDigits "111122"

// %%
let matchPasswordRules2 (password:string) = 
    hasExactlyTwoAdjacentDigits password && allDigitsIncreasing password

matchPasswordRules2 "111234"

// %%
getAllMatchingPasswords matchPasswordRules2 input[0] input[1]
|> Seq.length

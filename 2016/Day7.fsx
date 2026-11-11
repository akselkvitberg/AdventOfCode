// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 7 |> GetLines
input[..4]

// %%
let isAbba (input:string) = 
    match input with
    | Regex "(\w)([\w])\2\1" [c1;c2] when c1 <> c2 -> true
    | _ -> false

["aabb"; "abba"; "aaaa"] 
|> Seq.map isAbba

// %%
let supportsTls (input:string) =
    let split = input.Split('[',']')
    [0..2..split.Length] |> Seq.map (fun i -> split[i]) |> Seq.exists isAbba && 
    [1..2..split.Length-1] |> Seq.map (fun i -> split[i]) |> Seq.forall (isAbba >> not)
    

supportsTls "abba[mnop]qrst",
supportsTls "abcd[bddb]xyyx"

// %%
input
|> Seq.where supportsTls
|> Seq.length

// %%
open System.Text.RegularExpressions

Regex.Matches("zazbz[bzb]cdb", "(?=(\w)(\w)\1)")
                    |> Seq.map (fun m -> Seq.tail m.Groups.Values)
                    |> Seq.map (fun m -> m |> Seq.map (fun y -> y.Value) |> Seq.toList)
                    |> Seq.toList

// %%
let getAba line =
    let captures = Regex.Matches(line, "(?=(\w)(\w)\1)")
                    |> Seq.map (fun m -> Seq.tail m.Groups.Values)
                    |> Seq.map (fun m -> m |> Seq.map (fun y -> y.Value) |> Seq.toList)
                    |> Seq.toList
    captures

getAba "aba[bab]"

// %%
let supportsSsl (input:string) =
    let split = input.Split('[',']')
    let aba1 = [0..2..split.Length]     |> Seq.map (fun i -> split[i]) |> Seq.collect getAba |> Set.ofSeq
    let aba2 = [1..2..split.Length - 1] |> Seq.map (fun i -> split[i]) |> Seq.collect getAba |> Seq.map (fun arr -> List.rev arr) |> Set.ofSeq
    Set.intersect aba1 aba2 |> Set.count > 0


supportsSsl "zazbz[bzb]cdb"

// %%
input
|> Seq.where supportsSsl
|> Seq.length

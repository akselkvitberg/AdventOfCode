// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = "ahsbgdzn"
input

// %%
open System.Security.Cryptography

let MD5 (input:string) = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(input))).ToLower()

MD5 "abc18"

// %%
let has5ofCharacter (segment:string) (hash:string) =
    hash.Contains(segment)

let isNext1000Matching hashes segment =
    hashes |> Seq.exists (has5ofCharacter segment)

let isKey (hashes:string array) =
    let key = hashes[0]
    match key with
    | Regex "(.)\1\1" [c] -> isNext1000Matching (hashes[1..])  (new string(c[0], 5))
    | _ -> false

// %%
let getKey (hashingFunction) input =
    Seq.initInfinite (fun i -> i, hashingFunction $"{input}{i}")
    |> Seq.windowed 1000
    |> Seq.filter (fun hashes -> hashes |> Array.map snd |> isKey)
    |> Seq.skip 63
    |> Seq.head
    |> Array.head
    |> fst
    |> printfn "%d"
    
getKey MD5 "abc"

// %%
getKey MD5 input

// %%
let MD5v2 input =
    [|0..2016|] |> Array.fold (fun acc _ -> MD5 acc) input

MD5v2 "abc0"

// %%
getKey MD5v2 "abc"

// %%
getKey MD5v2 input

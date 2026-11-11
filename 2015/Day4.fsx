// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
open System.Security.Cryptography

#load "../utils.fsx"
open Utils
let input = GetData 4

// %%
let HashData input leadingZeros number =
    let combined = $"{input}{number}"
    let hash = combined 
                |> Encoding.ASCII.GetBytes
                |> MD5.HashData 
                |> Seq.take 3
                |> Seq.fold (fun state x -> state + System.String.Format("{0:X2}", x)) ""
    hash[0..(leadingZeros-1)] = new string('0', leadingZeros)


Seq.initInfinite (HashData input 5)
|> Seq.indexed
|> Seq.find (fun (i, r) -> r)
|> fst

// %%
Seq.initInfinite (HashData input 6)
|> Seq.indexed
|> Seq.find (fun (i, r) -> r)
|> fst

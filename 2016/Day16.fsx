// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 16
input

// %%
let toString data =
    data
    |> Seq.toArray
    |> fun d -> new String(d)

// %%
let rec reverseAndInvert requiredLength (input:char array) =
    let result = 
        input
        |> Seq.rev
        |> Seq.map (fun c -> if c = '1' then '0' else '1')
        |> Seq.append [|'0'|]
        |> Seq.append input
        |> Seq.toArray
    if result.Length < requiredLength then reverseAndInvert requiredLength result
    else result[..requiredLength-1]

"111100001010"
|> Seq.toArray
|> reverseAndInvert 30
|> toString

// %%
let checksumDigit [|a;b|] =
    if a = b then '1' else '0'

let rec checksum input =
    let result = 
        input
        |> Seq.chunkBySize 2
        |> Seq.map checksumDigit
        |> Seq.toArray
    if result.Length % 2 = 0 then checksum result
    else result
    

"110010110100"
|> Seq.toArray
|> checksum
|> toString

// %%
"10000"
|> Seq.toArray
|> reverseAndInvert 20
|> checksum
|> toString

// %%
input
|> Seq.toArray
|> reverseAndInvert 272
|> checksum
|> toString

// %%
input
|> Seq.toArray
|> reverseAndInvert 35651584
|> checksum
|> toString

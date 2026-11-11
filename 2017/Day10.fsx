// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 10 
let inputSequence = input |> Split "," |> Array.map int |> List.ofArray
input

// %%
let numbers = [|0..255|]
let exampleNumbers = [|0..4|]
let example = [3; 4; 1; 5]

// %%
type State = {
    Numbers: int array
    CurrentPosition: int
    SkipSize: int
}

// %%
let reverse (numbers: int array) currentPosition length = 
    let indexes = [|currentPosition..currentPosition + length - 1 |] |> Array.map (fun x -> x % numbers.Length)
    let revIndexes = Array.rev indexes
    let map = Array.zip indexes revIndexes |> Map.ofArray

    numbers |> Array.mapi (fun i n -> 
        match Map.tryFind i map with
        | Some p -> numbers[p]
        | None -> n
        )

reverse numbers 250 10

// %%
let rec hash (numbers: int array) currentPosition skipSize lengths =
    match lengths with 
    | [] -> numbers, currentPosition, skipSize
    | length::lengths -> 
        let numbers = reverse numbers currentPosition length
        hash numbers (currentPosition + length + skipSize) (skipSize + 1) lengths

hash exampleNumbers 0 0 example

// %%
hash numbers 0 0 inputSequence
|> fun (x,_,_) -> x[0] * x[1]

// %%
let stringToBytes (input:string) = Encoding.ASCII.GetBytes(input) |> List.ofArray |> List.map int |> fun x -> List.append x [17; 31; 73; 47; 23]
let bytesToHex (input: int array) = input |> Array.map byte |> Convert.ToHexStringLower

// %%
let hash2 (numbers: int array) input =
    let lengths = stringToBytes input
    [1..64]
    |> List.fold (fun (numbers, currentPosition, skipSize) i -> hash numbers currentPosition skipSize lengths ) (numbers, 0, 0)
    |> fun (x,_,_) -> x
    |> Array.chunkBySize 16
    |> Array.map (Array.reduce (^^^))
    |> bytesToHex

hash2 numbers ""

// %%
hash2 numbers "AoC 2017"

// %%
hash2 numbers input

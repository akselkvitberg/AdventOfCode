// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 3 |> GetLines
input[..3]

// %%
let parseChar =
    function
    | a when a >= 'a' && a <= 'z' -> (int a) - (int 'a') + 1
    | a -> (int a) - (int 'A') + 27
    
let parse (input:string) = input.ToCharArray() |> Array.map parseChar

[
    'a'
    'A'
    'b'
    'B'
]
|> Seq.map parseChar

// %%
let getCompartments (line:int array) = Set.ofArray line[..line.Length / 2 - 1], Set.ofArray line[line.Length/2..]
let getCommonItems (c1, c2) = Set.intersect c1 c2

"vJrwpWtwJgWrhcsFMMfFFhFp"
|> parse
|> getCompartments
|> getCommonItems

// %%
[
    "vJrwpWtwJgWrhcsFMMfFFhFp"
    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
    "PmmdzqPrVvPwwTWBwg"
    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
    "ttgJtRGJQctTZtZT"
    "CrZsJsPPZsGzwwsLwLmpwMDw"
]
|> Seq.map parse
|> Seq.map getCompartments
|> Seq.collect getCommonItems
|> Seq.sum

// %%
input
|> Seq.map parse
|> Seq.map getCompartments
|> Seq.collect getCommonItems
|> Seq.sum

// %%
[
    "vJrwpWtwJgWrhcsFMMfFFhFp"
    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
    "PmmdzqPrVvPwwTWBwg"
    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
    "ttgJtRGJQctTZtZT"
    "CrZsJsPPZsGzwwsLwLmpwMDw"
]
|> Seq.map parse
|> Seq.map Set.ofArray
|> Seq.chunkBySize 3
|> Seq.collect Set.intersectMany
|> Seq.sum

// %%
input
|> Seq.map parse
|> Seq.map Set.ofArray
|> Seq.chunkBySize 3
|> Seq.collect Set.intersectMany
|> Seq.sum

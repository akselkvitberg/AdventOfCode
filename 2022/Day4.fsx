// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 4 |> GetLines
input[..3]

// %%
let parse input =
    match input with
    | Regex "(\d+)-(\d+),(\d+)-(\d+)" [Int a; Int b; Int c; Int d] -> a,b,c,d

parse input[0]

// %%
let contains (a,b,c,d) =
    (a >= c && b <= d)
    || (c >= a && d <= b)

[
"2-4,6-8"
"2-3,4-5"
"5-7,7-9"
"2-8,3-7"
"6-6,4-6"
"2-6,4-8"
]
|> Seq.map parse
|> Seq.filter contains
|> Seq.length

// %%
input
|> Seq.map parse
|> Seq.filter contains
|> Seq.length

// %%
let overlaps (a,b,c,d) =
    (a >= c && a <= d) ||
    (b >= c && b <= d) ||
    (c >= a && c <= b) ||
    (d >= a && d <= b)

// %%
[
"2-4,6-8"
"2-3,4-5"
"5-7,7-9"
"2-8,3-7"
"6-6,4-6"
"2-6,4-8"
]
|> Seq.map parse
|> Seq.filter overlaps
|> Seq.length

// %%
input
|> Seq.map parse
|> Seq.filter overlaps
|> Seq.length

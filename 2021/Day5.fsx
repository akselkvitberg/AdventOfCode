// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
open System.Text.RegularExpressions

// %%
#load "../utils.fsx"
open Utils

let lines = GetData 5 |> GetLines

// %%
let parse line =
    let items = 
        Regex.Matches(line, @"-?\d+")
        |> Seq.map (fun x -> int x.Value)
        |> Seq.toList
    items[0], items[1], items[2], items[3]

lines[0..3] |> Seq.map parse

// %%
let conditionallyReverse a b list = if b < a then (Seq.rev list) else list
let line a b = [(min a b)..(max a b)] |> conditionallyReverse a b
let points (x1,y1,x2,y2) = 
    if x1 = x2 then line y1 y2 |> Seq.map (fun y -> x1, y) else
    if y1 = y2 then line x1 x2 |> Seq.map (fun x -> x, y1) else
    line y1 y2 |> Seq.zip (line x1 x2)

points (0,3,3,0)

// %%
let isStraight (x1,y1,x2,y2) = x1 = x2 || y1 = y2

// %%
lines
|> Seq.map parse
|> Seq.filter isStraight
|> Seq.collect points
|> Seq.groupBy id
|> Seq.filter (snd >> Seq.length >> (<)1)
|> Seq.length

// %%
lines
|> Seq.map parse
|> Seq.collect points
|> Seq.groupBy id
|> Seq.filter (snd >> Seq.length >> (<)1)
|> Seq.length

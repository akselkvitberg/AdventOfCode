// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let items = 
    GetData 6
    |> Seq.map int

// %%
items
|> Seq.countBy id

// %%
let nextStep (internalCounter, count) = 
    if internalCounter = 0 then [(6, count); (8, count)] else [(internalCounter-1, count)]


let fishes = items |> Seq.countBy id |> Seq.map (fun (a, b) -> a, (int64 b))

fishes
|> Seq.collect nextStep
|> Seq.collect nextStep

// %%
let combineGroups (groups:seq<int*int64>) = groups |> Seq.groupBy (fun (a,count) -> a) |> Seq.map (fun (key, groups) -> key, groups |> Seq.sumBy (fun (_,count) -> count))
let afterNdays n fishes = [1..n] |> Seq.fold (fun content i -> content |> Seq.collect nextStep |> combineGroups ) fishes
let countFishes (fish:seq<int*int64>) = fish |> Seq.sumBy (fun (_,count) -> count)

// %%
afterNdays 80 fishes |> countFishes

// %%
afterNdays 256 fishes |> countFishes

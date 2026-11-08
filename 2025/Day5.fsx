// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 5
let example = GetExample 5 1
example

// %%
type Range = int64 * int64

let isInRange value (min, max) =
    value >= min && value <= max

let size (min, max) =
    max - min + 1L

// %%
let parse input = 
    let [|fresh; available|] = input |> GetBlocks
    let freshIngredients =
        fresh
        |> GetLines
        |> Array.map (fun line -> Split "-" line |> fun [|a;b|] -> (int64 a, int64 b))

    let availableIngredients = 
        available
        |> GetLines
        |> Array.map int64

    freshIngredients, availableIngredients

parse example

// %%
let findAllAvailableFreshIngredients input =
    let freshIngredients, availableIngredients = parse input
    availableIngredients
    |> Array.filter (fun ingredient -> 
        freshIngredients
        |> Array.exists (isInRange ingredient)
    )
    |> Array.length

findAllAvailableFreshIngredients example

// %%
findAllAvailableFreshIngredients input

// %%
let unionRanges (ranges: Range[]) =
    let sortedRanges = 
        ranges
        |> Array.sortBy fst
        |> Array.toList

    let rec merge acc list =
        match list with 
        | [] -> acc
        | head::tail ->
            match acc with
            | [] -> merge [head] tail
            | last::rest -> 
                let (lastMin, lastMax) = last
                let (currentMin, currentMax) = head
                if currentMin <= lastMax + 1L then
                    let newMax = max lastMax currentMax
                    let newRange = (lastMin, newMax)
                    merge (newRange::rest) tail
                else
                    merge (head::acc) tail

    merge [] sortedRanges

let part2 input =
    let ranges, _ = parse input
    let mergedRanges = unionRanges ranges
    mergedRanges
    |> List.sumBy size

part2 example

// %%
part2 input

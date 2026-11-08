// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 3
let example = GetExample 3 1
example

// %%
open System.Text.RegularExpressions

let part1 input = 
    Regex.Matches(input, @"mul\((\d+),(\d+)\)")
    |> Seq.map (fun m -> m.Groups[1].Value, m.Groups[2].Value)
    |> Seq.sumBy (fun (a, b) -> int a * int b)

part1 example

// %%
part1 input

// %%
let example2 = GetExample 3 2
example2

// %%
let part2 (input:string) =
    let pattern = Regex("mul\((\d+),(\d+)\)|do\(\)|don't\(\)")
    let matches = 
        pattern.Matches(input)
        |> Seq.map (fun x -> x.Groups |> Seq.map _.Value |> Seq.toList)
        |> Seq.toList

    Seq.fold (fun (enable, total) value ->
        match (enable, value) with
        | _, ["do()";_;_] -> (true, total)
        | _, ["don't()";_;_] -> (false, total)
        | true, [_;a;b] -> (true, total + int a * int b)
        | _ -> (enable, total)
    ) (true, 0) matches
    |> snd

part2 example2

// %%
part2 input

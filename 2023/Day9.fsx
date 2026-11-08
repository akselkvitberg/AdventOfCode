// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 9

// %%
let example = GetExample 9 1
example

// %%
let parse input = input |> GetLines |> Array.map (Split " " >> Array.map int)
parse example

// %%
let getDiff values =
    if(Seq.forall ((=) 0) values) then None
    else Some (
        values,
        values
        |> Seq.pairwise
        |> Seq.map (fun (a,b) -> b - a)
        |> Seq.toArray
    )

let interpolate values =
    values |> Array.unfold (fun agg -> getDiff agg)

example |> parse |> fun x -> interpolate x[2]

// %%
type Direction = Forwards | Backwards
let extrapolateValues direction valueValues = 
    let valueSelector, extrapolationFunction = match direction with Forwards -> Array.last, (fun agg i -> i + agg) | _ -> Array.head, (fun agg i -> i - agg)
    valueValues
    |> Array.map valueSelector
    |> Array.rev
    |> Array.reduce extrapolationFunction

example |> parse |> fun x -> interpolate x[2] |> extrapolateValues Forwards

// %%
let extrapolate direction input =
    input
    |> parse
    |> Seq.map interpolate
    |> Seq.map (extrapolateValues direction)
    |> Seq.sum

extrapolate Forwards example

// %%
extrapolate Forwards input

// %%
extrapolate Backwards example

// %%
extrapolate Backwards input

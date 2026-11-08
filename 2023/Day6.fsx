// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 6

input

// %%
let example = GetExample 6 1
example

// %%
let parse input =
    match input with 
    | Regex "([\s\d]+)\s+Distance:([\s\d]+)" [LongArray times; LongArray distances ] -> distances |> Array.zip times

parse example

// %%
let winGame ((time:int64), (distance:int64)) =
    let getTravel holdTime = (time - holdTime) * holdTime

    Seq.initInfinite (fun i -> int64 i + 1L)
    |> Seq.map (fun time -> getTravel time)
    |> Seq.skipWhile (fun travel -> travel <= distance)
    |> Seq.takeWhile (fun travel -> travel > distance)
    |> Seq.length

winGame (7L,9L)

// %%
let getMargin input =
    input
    |> parse
    |> Array.map winGame 
    |> Array.reduce (*)

getMargin example

// %%
getMargin input

// %%
let getMarginOnlyOneGame (input:string) =
    input.Replace(" ", "")
    |> getMargin

getMarginOnlyOneGame example

// %%
getMarginOnlyOneGame input

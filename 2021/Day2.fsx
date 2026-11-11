// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let lines = GetData 2 |> GetLines
let items = lines
items[..3]

// %%
type Command = 
    | Forward of int
    | Down of int 
    | Up of int

// %%
let parse (input:string) = 
    let [|text; number|] = input.Split(" ")
    let distance = number |> int
    let command =
        match text with
            | "forward" -> Forward
            | "down" -> Down
            | "up" -> Up

    command distance

let commands = items |> Seq.map parse

// %%
type Position = { X: int; Y: int}

let applyCommand state command = 
    match command with
        | Forward d -> { state with X = state.X + d }
        | Down d -> { state with Y = state.Y + d }
        | Up d -> { state with Y = state.Y - d }

let state = {X = 0; Y = 0}

let result = commands |> Seq.fold applyCommand state
result.X * result.Y

// %%
type Position2 = { X: int; Y: int; Aim: int }

let applyCommand (state: Position2) command = 
    match command with
        | Forward d -> { state with
                            X = state.X + d
                            Y = state.Y + d * state.Aim
                       }
        | Down d -> { state with Aim = state.Aim + d }
        | Up d -> { state with Aim = state.Aim - d }

let state = {X = 0; Y = 0; Aim = 0 }

let result = commands |> Seq.fold applyCommand state
result.X * result.Y

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 3

type Position = {X:int; Y:int}

// %%
let Parse c = 
    match c with
    | '>' -> {X = 1; Y = 0}
    | '<' -> {X = -1; Y = 0}
    | 'v' -> {X = 0; Y = 1}
    | '^' -> {X = 0; Y = -1}

let directions = input |> Seq.map Parse

// %%
let Move santa position = 
    { santa with
        X = santa.X + position.X
        Y = santa.Y + position.Y
    }

// %%
directions 
|> Seq.scan Move {X=0; Y=0} 
|> Seq.distinct
|> Seq.length

// %%
let Move2 (active, passive) position =
    let newActive = Move active position
    (passive, newActive)

directions 
|> Seq.scan Move2 ({X=0; Y=0}, {X=0; Y=0})
|> Seq.collect (fun (a,b) -> [a;b])
|> Seq.distinct
|> Seq.length

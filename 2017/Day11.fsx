// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 11 |> Split ","
input

// %%
type HexPoint = { X: int; Y: int; Z: int }

// %%
let move (point: HexPoint) direction =
    match direction with
    | "n" -> { point with Y = point.Y + 1; Z = point.Z - 1 }
    | "ne" -> { point with X = point.X + 1; Z = point.Z - 1 }
    | "se" -> { point with X = point.X + 1; Y = point.Y - 1 }
    | "s" -> { point with Y = point.Y - 1; Z = point.Z + 1 }
    | "sw" -> { point with X = point.X - 1; Z = point.Z + 1 }
    | "nw" -> { point with X = point.X - 1; Y = point.Y + 1 }
    | _ -> point

// %%
let getDistance (point: HexPoint) =
    (abs point.X + abs point.Y + abs point.Z) / 2

// %%
input
|> Array.fold move { X = 0; Y = 0; Z = 0 }
|> getDistance

// %%
input
|> Array.scan move { X = 0; Y = 0; Z = 0 }
|> Array.map getDistance
|> Array.max

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 1 |> GetLines |> Array.map int
input

// %%
let getRequiredFuel mass = 
    (mass / 3) - 2

getRequiredFuel 100756

// %%
input
|> Array.map getRequiredFuel
|> Array.sum

// %%
let getRequiredFuel2 mass = 
    let massFunction mass = 
        let requiredFuel = (mass / 3) - 2
        if(requiredFuel <= 0) then None
        else Some (requiredFuel, requiredFuel)
    mass |> Seq.unfold massFunction |> Seq.sum

getRequiredFuel2 1000

// %%
input
|> Array.map getRequiredFuel2
|> Array.sum

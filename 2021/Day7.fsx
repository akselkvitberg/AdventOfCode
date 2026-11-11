// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
let example = [|16;1;2;0;4;2;7;1;2;14|]

// %%
example |> Seq.sort

// %%
let positionsToTry list = [Seq.min list .. Seq.max list]
positionsToTry example

// %%
let calculateFuelCostPerCrab position fuelCost list = 
    list |> Seq.map (fuelCost position)

let naiveFuelCost crab position = abs (crab - position)
calculateFuelCostPerCrab 3 naiveFuelCost example 

// %%
let calculateFuelCost list fuelCost position  = list |> calculateFuelCostPerCrab position fuelCost |> Seq.sum
calculateFuelCost example naiveFuelCost 2

// %%
let crabs = File.ReadAllText("input/7.txt").Split(",") |> Seq.map int
positionsToTry crabs |> Seq.map (calculateFuelCost crabs naiveFuelCost) |> Seq.min

// %%
let advancedFuelCost crab position = [1..abs (crab - position)] |> Seq.sum
positionsToTry crabs |> Seq.map (calculateFuelCost crabs advancedFuelCost) |> Seq.min

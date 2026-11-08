// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 10
let example = GetExample 10 1
example

// %%
type Machine = {
    Indicators: bool array
    Buttons: int array array
}

let split line =
    match line with
    | Regex "\[(.+)\] (.+) (\{.+\})" [indicators; buttons; jolts] -> indicators, buttons, jolts

let parseLine (indicators, buttons, jolts) =
    {
        Indicators = indicators |> Seq.map ((=) '#') |> Seq.toArray
        Buttons = buttons |> Matches "\(([\d?,]+)\)" |> Seq.map (Split "," >> Array.map int) |> Seq.toArray
    }

let parse input =
    GetLines input
    |> Array.map (split >> parseLine)

parse example

// %%
let solveMachine (machine: Machine) =
    let flip indicators positions =
        indicators |> Array.mapi (fun i v -> if positions |> Array.contains i then not v else v)
    
    let start = Array.init machine.Indicators.Length (fun _ -> false)

    let buttonPresses = 
        machine.Buttons
        |> allCombinations
        |> Seq.sortBy List.length
        |> Seq.pick (fun presses ->
            let finalIndicators = List.fold flip start presses
            if finalIndicators = machine.Indicators then Some (List.length presses) else None
        )
    buttonPresses

solveMachine (parse input).[0]

// %%
let part1 input =
    input
    |> parse
    |> Array.map solveMachine
    |> Array.sum
part1 example

// %%
part1 input

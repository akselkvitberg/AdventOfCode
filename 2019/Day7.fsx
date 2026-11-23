#load "../utils.fsx"
#load "Intcode.fsx"
open Utils
open Intcode

let input = GetData 7
// example 1 is the amplifier diagram; 2-4 are programs with max signals 43210, 54321 and 65210
let examples = [ for i in 2..4 -> GetExample 7 i ]
examples

// %%
/// Every order of five distinct values, one per amplifier
let phaseOrders values =
    [ for a in values do
      for b in values do
      for c in values do
      for d in values do
      for e in values do
          let phases = [ a; b; c; d; e ]
          if List.distinct phases = phases then phases ]

let runAmplifier program signal phase =
    execute [ phase; signal ] program |> List.head

let runChain program phases =
    phases |> List.fold (runAmplifier program) 0L

let part1 input =
    let program = parse input
    phaseOrders [ 0L .. 4L ]
    |> List.map (runChain program)
    |> List.max

examples |> List.map part1

// %%
part1 input

// %%
/// Pass the signal through every machine once.
/// Returns E's output and the updated machines, or None when a machine halts.
let rec runRound signal machines =
    match machines with
    | [] -> Some (signal, [])
    | machine :: rest ->
        match machine |> addInputs [ signal ] |> run with
        | Output (value, machine) ->
            runRound value rest
            |> Option.map (fun (signal, rest) -> signal, machine :: rest)
        | _ -> None

/// Keep sending the signal around until the machines halt; the last signal is the answer
let rec feedbackLoop signal machines =
    match runRound signal machines with
    | Some (signal, machines) -> feedbackLoop signal machines
    | None -> signal

let runFeedbackLoop program phases =
    phases
    |> List.map (fun phase -> create program |> addInputs [ phase ])
    |> feedbackLoop 0L

let part2 input =
    let program = parse input
    phaseOrders [ 5L .. 9L ]
    |> List.map (runFeedbackLoop program)
    |> List.max

// expected 139629729 and 18216
[ GetExample 7 6; GetExample 7 7 ] |> List.map part2

// %%
part2 input

// %%

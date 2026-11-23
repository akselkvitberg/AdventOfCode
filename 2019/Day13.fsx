#load "../utils.fsx"
#load "Intcode.fsx"
open Utils
open Intcode

let input = GetData 13
input
// %%

let part1 input =
    parse input
    |> execute []
    |> List.chunkBySize 3
    |> List.filter (fun l -> l[2] = 2L)
    |> List.length

part1 input

// %%

type Screen = Map<int64 * int64, int64>

let draw (screen:Screen) (outputs: int64 list) =
    outputs
    |> List.chunkBySize 3
    |> List.map (fun [x;y;id] -> (x,y),id)
    |> List.fold (fun screen (p,id) -> Map.add p id screen) screen

let joystick (screen:Screen) =
    let (x1,_) = Map.findKey (fun k v -> v = 4L) screen
    let (x2,_) = Map.findKey (fun k v -> v = 3L) screen
    sign(x1 - x2) |> int64 

let rec play machine (screen:Screen) =
    let outputs, stopped = runUntilBlocked machine
    let screen = draw screen outputs
    match stopped with
    | NeedsInput machine -> 
        let input = joystick screen
        let machine = machine |> addInputs [input]
        play machine screen
    | Halted _ -> screen |> Map.find (-1L, 0)

let part2 input =
    let program = parse input
    // free play
    program[0] <- 2L
    play (create program) Map.empty

// %%
part2 input
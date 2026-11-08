// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 1 |> GetLines
let example = GetExample 1 1 |> GetLines
example

// %%
let part1 input =
    let moveDial state input =
        match input with
        | Regex "R(\d+)" [Int steps] -> (state + steps) 
        | Regex "L(\d+)" [Int steps] -> (state - steps) 
        |> fun x -> x % 100

    input
    |> Array.scan moveDial 50
    |> Array.filter (fun x -> x = 0)
    |> Array.length

part1 example

// %%
part1 input

// %%
let part2 input =
    let moveDial lastMoves input =
        let state = Array.last lastMoves
        match input with
            | Regex "R(\d+)" [Int steps] -> [|state+1..1..(state + steps)|]
            | Regex "L(\d+)" [Int steps] -> [|state-1..(-1)..(state - steps)|]
        |> Array.map (fun x -> x % 100)
    
    input
    |> Array.scan moveDial [|50|]
    |> Array.concat
    |> Array.filter (fun x -> x = 0)
    |> Array.length

part2 example

// %%
part2 input

// C#
// [csharp cell, not converted]
// string[] input = [ "L68", "L30", "R48", "L5", "R60", "L55", "L1", "L99", "R14", "L82" ];
//
// var pos = 50;
// var count = 0;
// foreach (var move in input)
// {
//     var steps = int.Parse(move[1..]);
//     pos = move[0] switch {
//         'R' => pos += steps,
//         'L' => pos -= steps,
//     };
//     pos = pos % 100;
//     if (pos == 0) count++;
// }
// count

// [csharp cell, not converted]
// var pos = 50;
// var count = 0;
// foreach (var move in input)
// {
//     var steps = int.Parse(move[1..]);
//     var range = move[0] switch {
//         'R' => Enumerable.Range(pos + 1, steps),
//         'L' => Enumerable.Range(pos - steps, steps).Reverse(),
//     };
//     foreach (var p in range)
//     {
//         pos = p % 100;
//         if (pos == 0) count++;
//     }
// }
// count

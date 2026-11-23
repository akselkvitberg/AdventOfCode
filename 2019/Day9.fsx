#load "../utils.fsx"
#load "Intcode.fsx"
open Utils
open Intcode

let input = GetData 9

input

// %%
let part1 input =
    parse input |> execute [ 1L ]

part1 input

// %%
let part2 input =
    parse input |> execute [ 2L ]

part2 input

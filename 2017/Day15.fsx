// %%
#load "../utils.fsx"
open System
open Utils

let input = GetData 15
let example = """Generator A starts with 65
Generator B starts with 8921"""
example

// %%
let parse = function Regex "(\d+)\n.+?(\d+)" [ULong a; ULong b] -> a,b

parse example

// %%
let generator factor start =
    Seq.unfold (fun prev ->
        let x = (prev * factor) % 2147483647UL
        Some (x,x)
    ) start
    
generator 16807UL 65UL

// %%
let lowestValueMatch (a, b) = (65535UL &&& a) = (65535UL &&& b)

lowestValueMatch (245556042UL, 1431495498UL),
lowestValueMatch (1352636452UL, 285222916UL)

// %%

let part1 input =
    let startA,startB = parse input
    let a = generator 16807UL startA
    let b = generator 48271UL startB
    Seq.zip a b
    |> Seq.take 40_000_000
    |> Seq.where (lowestValueMatch)
    |> Seq.length
   
part1 example

// %%
part1 input

// %%
let part2 input =
    let startA,startB = parse input
    let a = generator 16807UL startA |> Seq.filter (fun x -> x % 4UL = 0UL)
    let b = generator 48271UL startB |> Seq.filter (fun x -> x % 8UL = 0UL)
    Seq.zip a b
    |> Seq.take 5_000_000
    |> Seq.where (lowestValueMatch)
    |> Seq.length
   
part2 example

// %%
part2 input
#load "../utils.fsx"
open System
open Utils

let input = GetData 16
input

// %%

type Instruction =
    | Spin of int
    | Exchange of int * int
    | Partner of string * string
    

let parseLine = function
    | Regex "s(.+)" [Int p] -> Spin p
    | Regex "x(.+)/(.+)" [Int a; Int b] -> Exchange (a,b)
    | Regex "p(.)/(.)" [a;b] -> Partner (a,b)
    | s -> failwith s

let parse input =
    input
    |> Split ","
    |> Array.map parseLine

parse input

// %%

let programs = ['a'..'p'] |> List.map string

let spin (state:string list) count =
    let head, tail = List.splitAt (16-count) state
    tail @ head
    
spin programs 3

// %%
let exchange (state:string list) a b =
    let pAtA = state[a]
    let pAtB = state[b]
    state |> List.updateAt a pAtB |> List.updateAt b pAtA

exchange programs 3 4
// %%
let partner (state:string list) a b =
    let iOfA = List.findIndex ((=)a) state
    let iOfB = List.findIndex ((=)b) state
    state |> List.updateAt iOfA b |> List.updateAt iOfB a

partner programs "a" "b"
// %%
let runInstruction state instruction =
    match instruction with
    | Spin p -> spin state p
    | Exchange (a,b) -> exchange state a b
    | Partner (a,b) -> partner state a b

runInstruction programs (Partner("a", "b"))

// %%
let runProgram programs instructions  =
    instructions
    |> Seq.fold runInstruction programs

let part1 = parse input |> runProgram programs |> String.concat ""
part1

// %%
let part2 =
    let instructions = parse input
    let states = Seq.unfold (fun s -> Some (s, runProgram s instructions)) programs |> Seq.cache
    let cycleLength = states |> Seq.skip 1 |> Seq.findIndex ((=) programs) |> (+) 1
    cycleLength
    
    states |> Seq.item (1_000_000_000 % cycleLength) |> String.concat ""
    
part2 
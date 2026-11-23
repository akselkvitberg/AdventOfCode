#load "../utils.fsx"
#load "Intcode.fsx"
open Utils

let input = GetData 12
let example1 = GetExample 12 1
let example2 = GetExample 12 4

example2

// %%

let parseLine line = match Matches "(-?\d+)" line with [Int x; Int y; Int z] -> [|x;y;z|] 

type Axis = { Pos: int array; Vel: int array }
// columnar representation of axis
let parse input =
    input
    |> GetLines
    |> Array.map parseLine
    |> Array.transpose
    |> Array.map (fun pos -> { Pos = pos; Vel = Array.zeroCreate pos.Length }) 

parse example1 

// %%

let gravity (positions: int array) i =
    positions |> Array.sumBy (fun p -> sign (p - positions[i]))

let stepAxis axis =
    let vel = axis.Vel |> Array.mapi (fun i v -> v + gravity axis.Pos i)
    let pos = Array.map2 (fun x dx -> x + dx) axis.Pos vel
    { Pos = pos; Vel = vel }

// %%
let calculateEnergy axes =
    let perMoon values = values |> Array.transpose |> Array.map (Array.sumBy abs)
    let potential = axes |> Array.map _.Pos |> perMoon
    let kinetic = axes |> Array.map _.Vel |> perMoon
    Array.map2 (fun p k -> p * k) potential kinetic |> Array.sum
    
// %%
let rec part1 steps input =
    let axis = parse input
    [1..steps]
    |> List.fold (fun axis _ -> axis |> Array.map stepAxis) axis
    |> calculateEnergy
    
part1 100 example2

// %%
part1 1000 input

// %%

let period start =
    let rec loop axis steps =
        let axis = stepAxis axis
        if axis = start then steps else loop axis (steps + 1L)
    loop start 1L

let rec gcd (a: int64) b = if b = 0L then a else gcd b (a % b)
let lcm a b = a / gcd a b * b

let part2 input = parse input |> Array.map period |> Array.reduce lcm
part2 input
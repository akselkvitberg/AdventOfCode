#load "../utils.fsx"
open Utils

let input = GetData 11 |> int
let example1 = 18
let example2 = 42
input

// %%
let powerLevel serialNumber (x,y) =
    let rackId = x + 10
    let power = rackId * y + serialNumber
    let power = power * rackId
    let power = power / 100 % 10
    power - 5

powerLevel 8 (3,5)

// %%
let createGrid serialNumber =
    Array2D.init 301 301 (fun y x -> if x = 0 || y = 0 then 0 else powerLevel serialNumber (x,y))

createGrid 8

// %%
let sum (grid:int[,]) (x,y)=
    seq {
        for iy in y .. y + 2 do
        for ix in x .. x + 2 do
            grid[iy, ix]
    } |> Seq.sum
    
let part1 input =
    let grid = createGrid input
    seq {
        for y in 1..298 do
        for x in 1..298 do
            x,y
    }
    |> Seq.maxBy (sum grid)
    |> fun (x, y) -> $"{x},{y}"

part1 example1, part1 example2

// %%
part1 input

// %%

let summedArea serial =
    let t = Array2D.zeroCreate 301 301
    for y in 1..300 do
        for x in 1..300 do
            t[y, x] <- powerLevel serial (x, y) + t[y - 1, x] + t[y, x - 1] - t[y - 1, x - 1]
    t

let squareSum (t: int[,]) size (x,y) =
    let x2 = x + size - 1
    let y2 = y + size - 1
    t[y2, x2] - t[y - 1, x2] - t[y2, x - 1] + t[y - 1, x - 1]

let bestSquare table size =
    seq {
        for y in 1..301 - size do
        for x in 1..301 - size do
            (x,y, size), squareSum table size (x,y) 
    }
    |> Seq.maxBy snd

let part2 input =
    let table = summedArea input 
    [1..300]
    |> List.map (bestSquare table)
    |> List.maxBy snd
    |> fun ((x, y, size), _) -> $"{x},{y},{size}"

part2 example1, part2 example2
// %%
part2 input
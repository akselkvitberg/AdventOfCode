// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 14
let example = "flqrgnkx"
example

// %%
let knotHash (input: string) =
    let lengths = Array.append (Encoding.ASCII.GetBytes input |> Array.map int) [| 17; 31; 73; 47; 23 |]
    let numbers = [| 0..255 |]
    let mutable position = 0
    let mutable skipSize = 0
    for _ in 1..64 do
        for length in lengths do
            for i in 0 .. length / 2 - 1 do
                let a = (position + i) % 256
                let b = (position + length - 1 - i) % 256
                let tmp = numbers[a]
                numbers[a] <- numbers[b]
                numbers[b] <- tmp
            position <- (position + length + skipSize) % 256
            skipSize <- skipSize + 1
    numbers
    |> Array.chunkBySize 16
    |> Array.map (Array.reduce (^^^) >> byte)

let exampleHash = knotHash $"{example}-0"
exampleHash

// %%
let hexToBits (c:byte) = Convert.ToString(c, 2).PadLeft(8,'0')
let hashToBits (hash: byte array) = hash |> Seq.collect hexToBits |> Seq.indexed |> Seq.where (fun (i,v) -> v = '1') |> Seq.toArray

hashToBits exampleHash
    //Convert.ToString(int string c)
// %%
let getRow input row = knotHash $"{input}-{row}" |> hashToBits
    
getRow example 0
     

// %%
let countUsedCells input =
    [|0..127|] |> Array.collect (getRow input) |> Array.length

countUsedCells example

// %%
countUsedCells input

// %%
let buildGrid input =
    [for y in 0..127 do
        getRow input y |> Seq.map (fun (x,v) -> (x,y))
    ]
    |> Seq.collect id
    |> Set.ofSeq
    
buildGrid example

// %%
let neighbours (y,x) = [ (y-1,x); (y+1,x); (y,x-1); (y,x+1) ]

let rec removeRegion pos (grid:Set<int*int>)  =
    if Set.contains pos grid then
        let grid' = Set.remove pos grid
        neighbours pos |> List.fold (fun p g -> removeRegion g p) grid'
    else
        grid

buildGrid example |> removeRegion (0,0)

// %%
let countRegions input =
    let grid = buildGrid input
    
    let rec count remaining acc =
        let firstElement = Set.minElement remaining
        let rem' = removeRegion firstElement remaining
        if Set.isEmpty rem'
            then acc
            else count rem' (acc+1)
    
    count grid 1

countRegions example

// %%
countRegions input
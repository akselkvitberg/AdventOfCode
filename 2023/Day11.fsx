// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 11

// %%
let example = GetExample 11 1
example

// %%
let parse (input:string) = 
    let lines = input |> GetLines
    let height, width = lines.Length, lines[0].Length
    [ 
        for y in 0 .. height - 1 do
        for x in 0..width - 1 do
        if lines[y][x] = '#' then int64 x, int64 y
    ]

parse example

// %%
let expand expansionRate pos =
    let max = pos |> Set.maxElement

    Array.unfold(fun state -> 
        let index, grow = state
        let grow' = if Set.contains index pos then grow else grow + (expansionRate - 1L)
        let state' = index + 1L, grow'
        if index > max then None
        else Some ((index, grow'), state')
    ) (0L, 0L)
    |> Map.ofArray

[0;2;3;5;7;10] |> List.map int64 |> set |> expand 2L

// %%
let correct (expansionRate:int64) galaxies = 
    let xs = galaxies |> List.map fst |> set
    let ys = galaxies |> List.map snd |> set
    let xMax = xs |> Set.maxElement
    let yMax = ys |> Set.maxElement

    let xExpand = expand expansionRate xs
    let yExpand = expand expansionRate ys
    
    galaxies |> List.map (fun (x,y) -> x + xExpand[x], y + yExpand[y])

let getGalaxies expansionRate input =
    input |> parse |> correct expansionRate

getGalaxies 2 example

// %%
let findShortestPath (x1:int64,y1:int64) (x2:int64,y2:int64) = abs (x1 - x2) + abs (y1 - y2)
findShortestPath (4,0) (12,7), findShortestPath (12,7) (4,0)

// %%
let rec getAllPairings list =
    [
        match list with
        | [] -> ()
        | x::xs -> 
            yield! List.map (fun item -> (x,item)) xs
            yield! getAllPairings xs
    ]

example |> getGalaxies 1 |> getAllPairings |> List.length

// %%
let getSumOfAllShortestPaths expansionRate input = 
    input
    |> getGalaxies expansionRate
    |> getAllPairings
    |> List.map (fun (p1,p2) -> findShortestPath p1 p2)
    |> List.sum

getSumOfAllShortestPaths 2 example

// %%
getSumOfAllShortestPaths 2 input

// %%
getSumOfAllShortestPaths 100 example

// %%
getSumOfAllShortestPaths 1000000 input

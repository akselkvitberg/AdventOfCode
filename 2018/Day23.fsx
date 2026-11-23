#load "../utils.fsx"
open Utils

let input = GetData 23
let example1 = GetExample 23 1
let example2 = GetExample 23 2
example1, example2

// %%

type Nanobot = {
    X: int
    Y: int
    Z: int
    R: int
}

let parseNanobot line =
    match Matches "(-?\d+)" line with
    | [Int x; Int y; Int z; Int r] ->
        {X = x; Y = y; Z = z; R = r}
    | _ -> failwith line
    
let parse = GetLines >> Array.map parseNanobot

parse example1

// %%

let distanceBetween (aX, aY, aZ) (bX, bY, bZ) =
    abs (aX - bX) +
    abs (aY - bY) +
    abs (aZ - bZ)

let isInRangeOf source bot =
    distanceBetween (source.X, source.Y, source.Z) (bot.X, bot.Y, bot.Z) <= source.R

let part1 input =
    let bots = parse input
    let largestBot = Array.maxBy _.R bots
    bots
    |> Array.filter (isInRangeOf largestBot)
    |> Array.length
    
    
part1 example1


// %%
part1 input

// %%

type Cube = {X: int; Y: int; Z: int; Size: int; Bots: Nanobot array}
// negative count, distance to zero, size, cube
type State = int * int * int * Cube

module Cube =
    
    let distanceToZero cube =
        distanceBetween (cube.X, cube.Y, cube.Z) (0,0,0)
    
    let axisDistance v lo hi =
        if v < lo then lo - v
        elif v > hi then v - hi
        else 0
        
    let isWithin (cube:Cube) (bot:Nanobot) =
        let hi v = v + cube.Size - 1
        let dist = axisDistance bot.X cube.X (hi cube.X)
                    + axisDistance bot.Y cube.Y (hi cube.Y)
                    + axisDistance bot.Z cube.Z (hi cube.Z)
        dist <= bot.R
    
    let getTouchingBots cube bots =
        bots |> Array.filter (isWithin cube)
        
    let scoreCube (cube: Cube) =
        let botsInsideCube = cube.Bots
        let distanceToZero = distanceBetween (cube.X, cube.Y, cube.Z) (0,0,0)
        -botsInsideCube.Length, distanceToZero, cube.Size, cube
    
    let splitCube (cube:Cube) =
        let half = cube.Size / 2
        [
            for x in 0..1 do
            for y in 0..1 do
            for z in 0..1 do
                {X = cube.X + x * half
                 Y = cube.Y + y * half
                 Z = cube.Z + z * half
                 Size = half
                 Bots = [||]}
        ]
        |> List.map (fun x -> {x with Bots = getTouchingBots x cube.Bots})
        |> List.map scoreCube
        |> Set.ofList
        
    let findBoundingBox (bots: Nanobot array) =
        let coords = bots |> Array.collect (fun b -> [| b.X; b.Y; b.Z |])
        let low = min 0 (Array.min coords)
        let hi = max 0 (Array.max coords)
        let span = hi - low + 1
        
        let rec powerOfTwoAtLeast n size =
            if size >= n then size else powerOfTwoAtLeast n (size * 2)
        
        let size = powerOfTwoAtLeast span 1
        {X = low; Y = low; Z = low; Size = size; Bots = bots}

open Cube

let findBestCube bots =
    let startingCube = Cube.findBoundingBox bots
       
    let rec loop (queue: Set<State>) =
        let (_,_,_,cube) as item = Set.minElement queue
        let queue = Set.remove item queue
        
        if cube.Size = 1 then cube
        else
            let cubes = splitCube cube
            let queue = Set.union queue cubes
            loop queue
    
    let state = scoreCube startingCube
    loop (Set.singleton state)
        
let part2 input =
    let bots = parse input
    let bestCube = findBestCube bots
    distanceToZero bestCube
    
part2 example2

// %%
part2 input

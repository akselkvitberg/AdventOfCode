#load "../utils.fsx"
open System
open Utils

let input = GetData 10

// %%
let example = """
.#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..
"""

// %%
let getAstroidLocations data =
    data
    |> GetLines
    |> Array.map Seq.toArray
    |> array2D
    |> Array2D.mapi (fun y x i -> (x,y),i)
    |> Seq.cast<((int*int)*char)>
    |> Seq.filter (fun (_,i) -> i = '#')
    |> Seq.map fst
    |> Seq.toArray

getAstroidLocations example

// %%
let rec gcd a b = if b = 0 then abs a else gcd b (a % b)

let getAstroidsInView (pX, pY) astroids = 
    astroids
    |> Array.filter (fun (x,y) -> x <> pX || y <> pY)
    |> Array.map (fun (x,y) -> (pX - x), (pY - y))
    |> Array.map (fun (x,y) -> x / gcd x y, y / gcd x y)
    |> Array.distinct

example
|> getAstroidLocations
|> getAstroidsInView (6,3)
|> Array.length

// %%
let getBestLocation input = 
    let astroids = 
        input
        |> getAstroidLocations
    astroids 
    |> Array.map (fun pos -> getAstroidsInView pos astroids |> Array.length, pos)
    |> Array.max

getBestLocation example
|> fst

// %%
getBestLocation input
|> fst

// PART 2
// %%
/// Clockwise - up = 0
let angle (sX, sY) (x, y) =
    let a = Math.Atan2(float (x - sX), float (sY - y))
    if a < 0.0 then a + 2.0 * Math.PI else a

let direction (sX, sY) (x, y) =
    let dx, dy = x - sX, y - sY
    let d = gcd dx dy
    dx / d, dy / d

let distance (sX, sY) (x, y) = abs (x - sX) + abs (y - sY)

let vaporizationOrder station astroids =
    astroids
    |> Array.filter (fun a -> a <> station)
    |> Array.groupBy (direction station)
    |> Array.collect (fun (_, group) ->
        group
        |> Array.sortBy (distance station)
        |> Array.mapi (fun turn a -> (turn, angle station a), a))
    |> Array.sortBy fst
    |> Array.map snd

let part2 input =
    let astroids = getAstroidLocations input
    let _, station = getBestLocation input
    let x, y = (vaporizationOrder station astroids)[199]
    x * 100 + y

// expected 802
part2 (GetExample 10 7)

// %%
part2 input

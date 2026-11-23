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
let getAngleOfAstroid (px,py) (ax,ay) =
    Math.Sinh (float (ax-px) / float (ay-py))

getAngleOfAstroid (0,0) (1,0)

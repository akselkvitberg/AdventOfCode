// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 18 |> GetLines

// %%
let grid = 
    [| for row in input -> 
        [| for col in row -> 
            if col = '#' 
                then true 
                else false
        |] 
    |] |> array2D

// %%
let GetNeighbours (grid:bool[,]) x y = 
    grid[x-1..x+1,y-1..y+1]
    |> Seq.cast<bool>
    |> Seq.filter id
    |> Seq.length


let GetNextState (grid:bool[,]) x y state =
    let neighbours = GetNeighbours grid x y
    match (state, neighbours) with
    | (true, 3) -> true
    | (true, 4) -> true
    | (false, 3) -> true
    | (_, _) -> false

let MoveNext (grid:bool[,]) = 
    grid |> Array2D.mapi (GetNextState grid)

grid 
|> MoveNext
|> MoveNext
|> MoveNext
|> MoveNext
|> Seq.cast<bool>
|> Seq.filter id
|> Seq.length

// %%
[1..100]
|> Seq.fold (fun grid _ -> MoveNext grid) grid
|> Seq.cast<bool>
|> Seq.filter id
|> Seq.length

// %%
let fixLights (grid:bool[,]) =
    grid[0,0] <- true
    grid[0,99] <- true
    grid[99,0] <- true
    grid[99,99] <- true
    grid

[1..100]
|> Seq.fold (fun grid _ -> MoveNext grid |> fixLights) (grid |> fixLights)
|> Seq.cast<bool>
|> Seq.filter id
|> Seq.length

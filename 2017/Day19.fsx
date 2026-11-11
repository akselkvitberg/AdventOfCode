#load "../utils.fsx"
open System
open Utils

let input = GetRawData 19
let example = GetRawExample 19 1
example

// %%


let solve (input: string)=
    let grid = input |> GetRawLines
    let at (x,y) = grid[y][x]
    
    let move (x,y) (dx,dy) = x + dx, y+dy
    
    let rec go pos dir = seq {
        match at pos with
        | ' ' -> ()
        | '|' | '-' ->
            yield None
            yield! go (move pos dir) dir
        | '+' ->
            let dx,dy = dir
            let dir' = [dy, -dx; -dy, dx] |> List.find (fun d -> at (move pos d) <> ' ')
            yield None
            yield! go (move pos dir') dir'
        | letter ->
            yield Some letter
            yield! go (move pos dir) dir
    }
    
    let start = grid[0].IndexOf '|', 0
    let steps = go start (0,1) |> Seq.toArray
    steps

solve example
            
            

        

// %%
let part1 input =
    solve input
    |> Array.choose id
    |> String.Concat
    
part1 example 

// %%
part1 input

// %%
let part2 input =
    solve input
    |> Array.length

part2 example

// %%
part2 input
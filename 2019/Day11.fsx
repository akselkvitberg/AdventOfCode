#load "../utils.fsx"
#load "Intcode.fsx"
open Utils
open Intcode

let input = GetData 11
input

// %%

type Vec2 = int * int

type State = {
    Machine: Machine
    Position: Vec2
    Direction: Vec2
    Panels: Map<Vec2, int64>
}

let turnLeft  (dx, dy) = (dy, -dx)
let turnRight (dx, dy) = (-dy, dx)
let move (x, y) (dx, dy) = x + dx, y + dy

let rec step (state:State) =
    let color = state.Panels |> Map.tryFind state.Position |> Option.defaultValue 0L
    let outputs, stopped = state.Machine |> addInputs [color] |> runUntilBlocked
    
    match outputs with
    | [paint; turn] ->
        let panels = state.Panels |> Map.add state.Position paint
        let direction = (if turn = 0L then turnLeft else turnRight) state.Direction
        let position = move state.Position direction
        match stopped with
        | NeedsInput machine -> step { Panels = panels
                                       Direction = direction
                                       Position = position
                                       Machine = machine }
        | _ -> panels
    | _ -> state.Panels 

// %%
let part1 input =
    { Machine = create (parse input)
      Position = (0,0)
      Direction = (0, -1)
      Panels = Map.empty }
    |> step
    |> Map.count
    
part1 input

// %%
let render (panels: Map<Vec2, int64>) =
    let white = panels |> Map.filter (fun _ colour -> colour = 1L) |> Map.keys |> Seq.toList
    let xs = white |> List.map fst
    let ys = white |> List.map snd
    [ for y in List.min ys .. List.max ys ->
        System.String [| for x in List.min xs .. List.max xs ->
                           if Map.tryFind (x, y) panels = Some 1L then '█' else ' ' |] ]
    |> String.concat "\n"

let part2 input =
    { Machine = create (parse input)
      Position = (0, 0)
      Direction = (0, -1)
      Panels = Map [ ((0, 0), 1L) ] }
    |> step
    |> render

part2 input |> printfn "%s"

#load "../utils.fsx"
open System.Collections.Generic
open System.Diagnostics.Tracing
open Utils

let input = GetData 20

/// Examples from the puzzle text, with the doors to the furthest room
let examples =
    [ "^WNE$"
      "^ENWWW(NEEE|SSE(EE|N))$"
      "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$"
      "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$"
      "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$" ]

// %%
type State =
    {
        Pos: int*int
        Stack: (int*int) list
        Dist: Map<int*int, int>
    }

let move (x, y) = function
    | 'N' -> x, y - 1
    | 'S' -> x, y + 1
    | 'E' -> x + 1, y
    | 'W'   -> x - 1, y
    | x -> failwith $"{x}"
    
let step state c =
    match c with
    | 'N' | 'E' | 'S' | 'W' ->
        let next = move state.Pos c
        let d = state.Dist[state.Pos] + 1
        
        // keep better dist
        let dist = state.Dist |> Map.change next (function Some old -> Some (min old d) | None -> Some d)
        { state with Pos = next; Dist = dist }
    | '(' -> { state with Stack = state.Pos :: state.Stack }
    | '|' -> { state with Pos = List.head state.Stack }
    | ')' -> { state with Pos = List.head state.Stack; Stack = List.tail state.Stack }
    | _   -> state

let distances regex =
    let start = { Pos = (0, 0); Stack = []; Dist = Map.ofList [ (0, 0), 0 ] }
    (regex |> Seq.fold step start).Dist

let part1 input =
    distances input
    |> Map.values |> Seq.max

examples |> List.map part1
// %%
part1 input

// %%
let part2 input =
    distances input
    |> Map.values |> Seq.filter (fun x -> x >= 1000) |> Seq.length

// %%
part2 input
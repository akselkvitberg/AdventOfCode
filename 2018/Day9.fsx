#load "../utils.fsx"
open Utils

let input = GetData 9
let examples = [
    "10 players; last marble is worth 1618 points: high score is 8317"
    "13 players; last marble is worth 7999 points: high score is 146373"
    "17 players; last marble is worth 1104 points: high score is 2764"
    "21 players; last marble is worth 6111 points: high score is 54718"
    "30 players; last marble is worth 5807 points: high score is 37305"
]
examples

// %%
let parse =
    function Regex "(\d+).+worth (\d+) points" [Long players; Long lastMarble] -> players, lastMarble
    
examples |> List.map parse

// %%
type Circle = { Left: int64 list; Current: int64; Right: int64 list }

let rec clockwise c =
    match c.Right with
    | x :: rest -> { Left = c.Current :: c.Left; Current = x; Right = rest }
    | [] when c.Left.IsEmpty -> c
    | [] -> clockwise { c with Left = []; Right = List.rev c.Left }

let rec counterClockwise c =
    match c.Left with
    | x :: rest -> { Left = rest; Current = x; Right = c.Current :: c.Right }
    | [] when c.Right.IsEmpty -> c
    | [] -> counterClockwise { c with Left = List.rev c.Right; Right = [] }

let insert m c = { c with Left = c.Current :: c.Left; Current = m }
let remove c =
    match c.Right with
    | x :: rest -> c.Current, {c with Current = x; Right = rest}
    | [] -> let r = List.rev c.Left
            c.Current, {Left = []; Current = r.Head; Right = r.Tail}

// %%
let play (players,lastMarble) =
    let step (circle, scores) marble =
        if marble % 23L = 0L then
            let back = Seq.fold (fun c _ -> counterClockwise c) circle (seq{1..7})
            let removed, circle = remove back
            let p = marble % players
            let old = Map.tryFind p scores |> Option.defaultValue 0L
            circle, Map.add p (old + marble + removed) scores
        else
            (circle |> clockwise |> insert marble), scores
    let _, scores = Seq.fold step ({ Left= []; Current =0L; Right = [] }, Map.empty) (seq {1L .. lastMarble})
    scores
    |> Map.values |> Seq.max

// %%
let part1 input =
    parse input |> play

List.map part1 examples

// %%
part1 input

// %%
let part2 input =
    let (players, lastMarble) = parse input
    play (players, lastMarble * 100L)

List.map part2 examples

// %%
part2 input
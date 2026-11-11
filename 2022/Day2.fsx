// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 2 |> GetLines
input[0..3]

// %%
type Move =
| Rock
| Paper
| Scissors

type Result =
| Win
| Draw
| Loss

// %%
let parseChar =
    function
    | 'A'
    | 'X' -> Rock
    | 'B'
    | 'Y' -> Paper
    | 'C'
    | _ -> Scissors

let parse (line:string) = (parseChar line[0]), (parseChar line[2])

parse "A Y"

// %%
let isWin move response =
    match move,response with
    | Rock, Rock
    | Paper, Paper
    | Scissors, Scissors -> Draw
    | Rock, Paper
    | Paper, Scissors
    | Scissors, Rock -> Win
    | _ -> Loss

isWin Rock Paper

// %%
let scoreResult =
    function
    | Win -> 6
    | Draw -> 3
    | Loss -> 0

let scoreMove =
    function
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3

let score (move,response) = scoreResult (isWin move response) + (scoreMove response)

score (Rock,Paper)

// %%
input
|> Seq.map parse
|> Seq.map score
|> Seq.sum

// %%
let parse2 (line:string) = 
    let move = parseChar line[0]
    let response = 
        match line[2], move with
        | 'X', Rock -> Scissors
        | 'X', Paper -> Rock
        | 'X', Scissors -> Paper
        | 'Z', Rock -> Paper
        | 'Z', Paper -> Scissors
        | 'Z', Scissors -> Rock
        | _,_ -> move
    move, response

parse2 "A X"

// %%
input
|> Seq.map parse2
|> Seq.map score
|> Seq.sum

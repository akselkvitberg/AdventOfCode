// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 15 |> GetLines |> Seq.toList
input

// %%
type Disc = {
    DiscId:int
    Positions:int
    Position0:int
}

// %%
let parse line = 
    match line with
    | Regex "Disc #(\d) has (\d+) positions; at time=0, it is at position (\d+)" [Int discId; Int positionCount; Int position0 ] -> 
        {DiscId = discId; Positions = positionCount; Position0 = position0 }
    | _ -> failwith line

let discs = input |> List.map parse
discs

// %%
let positionAt t disc =
    (disc.Position0 + t + disc.DiscId) % disc.Positions

[0..10]
|> Seq.map (fun t -> positionAt t discs[5])

// %%
let wouldPass t disc =
    positionAt t disc = 0

wouldPass 2 discs[0]

// %%
[0..1000000]
|> Seq.find (fun t ->
    discs
    |> Seq.forall (wouldPass t)
)

// %%
let newDiscs = {DiscId = 7; Positions = 11; Position0 = 0}::discs
newDiscs

// %%
[0..10000000]
|> Seq.find (fun t ->
    newDiscs
    |> Seq.forall (wouldPass t)
)

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 18
input

// %%
type Tile = 
    | Safe
    | Trap

// %%
let tileToChar = 
    function
    | Safe -> '.'
    | _ -> '^'

let toString input = 
    input 
    |> Seq.map tileToChar
    |> Seq.toArray
    |> String

[Safe;Trap;Safe;Trap] |> toString

// %%
let parse input = 
    input
    |> Seq.map (fun c -> if c = '.' then Safe else Trap)
    |> Seq.toArray

input
|> parse
|> toString

// %%
let getTileStatus parent =
    match parent with
    | [|Trap;Trap;Safe|] -> Trap
    | [|Safe;Trap;Trap|] -> Trap
    | [|Trap;Safe;Safe|] -> Trap
    | [|Safe;Safe;Trap|] -> Trap
    |_ -> Safe

let getNextLine parent =
    parent
    |> fun a -> [[|Safe|]; a; [|Safe|]]
    |> Seq.concat
    |> Seq.windowed 3
    |> Seq.map getTileStatus
    |> Seq.toArray
"..^^."
|> parse
|> getNextLine
|> toString

// %%
let getRoom rows input =
    [0..rows-2]
    |> Seq.scan (fun acc _ -> getNextLine acc) (input |> parse)

getRoom 10 ".^^.^.^^^^"
|> Seq.collect id
|> Seq.countBy id

// %%
getRoom 40 input
|> Seq.collect id
|> Seq.countBy id

// %%
getRoom 400000 input
|> Seq.collect id
|> Seq.countBy id

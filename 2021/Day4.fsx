// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let lines = GetData 4 |> GetLines

let numbers = lines[0].Split(",") |> Seq.map int

let boardData = lines |> Seq.skip 1

// %%
type Number = Number of int * bool
type Board = Board of Number[,]

// %%
//let splitString (line:string) (split:string) = line.Split(split, StringSplitOptions.RemoveEmptyEntries)

let splitRows (board:string) = 
    board.Split("\n", StringSplitOptions.RemoveEmptyEntries)

let parseRows (row:string) =
    row.Split(" ", StringSplitOptions.RemoveEmptyEntries)

let parseNumbers input = Number(int input, false)

let parseBoard boardString = 
    boardString
    |> splitRows
    |> Seq.map parseRows
    |> array2D
    |> Array2D.map parseNumbers

let boards = boardData |> Seq.map parseBoard |> Seq.map Board |> Seq.toArray

// %%
let mark number (Board board) = 
    Array2D.map (fun (Number (num, marked)) -> Number (num, marked || (num = number))) board
    |> Board

// %%
let check (Board board) = 
    let check vector = 
        Seq.forall (fun (Number (_, marked)) -> marked) vector
    [|0..(Array2D.length1 board - 1)|]
    |> Array.exists (fun i -> board[i,*] |> check)
    ||
    [|0..(Array2D.length2 board - 1)|]
    |> Array.exists (fun j -> board.[*, j] |> check)

let markedBoard = [38; 80; 23; 60; 82; 25] |> Seq.fold (fun acc num -> mark num acc) boards[0]
check markedBoard

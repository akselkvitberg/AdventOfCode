// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 9
let example = GetExample 9 1
example

// %%
let parse (input:string) =
    input.ToCharArray()
    |> Array.map (fun c -> int c - int '0')
    |> Array.toList
    |> List.mapi (fun i length -> if i % 2 = 0 then length, i / 2 else length, -1)
    
//parse example

// %%
let part1 input = 
    let layout = 
        parse input
        |> List.collect (function | (length, -1) -> [for i in 1..length do -1] | (length, id) -> [for i in 1..length do id])

    let reverse = layout |> List.rev |> List.filter (fun i -> i >= 0)

    let length = reverse.Length

    let rec defrag (checksum:int64) position layout reverse =
        if (position >= length) then checksum
        else
        match layout, reverse with
        | -1::ls, id::rs
        | id::ls, rs -> defrag (checksum + (int64 id) * (int64 position)) (position + 1) ls rs

    defrag 0L 0 layout reverse

part1 example

// %%
part1 input

// %%
let calculateChecksum (length, id, position) =
    [0..length - 1] |> List.sumBy (fun i -> (int64 position + int64 i) * int64 id)
calculateChecksum (2, 9, 2)

// %%
let rec defrag result freeSpaces rev =
    match rev with
    | [] -> result
    | (length, id, position)::rs -> 
        match freeSpaces |> List.tryFindIndex (fun (l,_,pos) -> l >= length && pos < position) with
        | None -> 
            let result = (length, id, position) :: result
            defrag result freeSpaces rs
        | Some index -> 
            let (l, _, pos) = freeSpaces.[index]
            let result = (length, id, pos) :: result
            if l = length then
                defrag result (freeSpaces |> List.removeAt index) rs
            else
                defrag result (freeSpaces |> List.updateAt index (l-length, -1, pos+length)) rs

let part2 input =

    let layout = 
        parse input
        |> List.scan (fun (length, id, position) (l, i) -> l, i, length + position) (0, 0, 0)
        |> List.skip 1

    let data, freeSpaces = layout |> List.partition (fun (length, id, i) -> id >= 0)

    let revData = data |> List.rev

    defrag [] freeSpaces revData
    |> List.sortBy (fun (_,_,pos) -> pos)
    |> List.sumBy calculateChecksum

part2 example

// %%
part2 input

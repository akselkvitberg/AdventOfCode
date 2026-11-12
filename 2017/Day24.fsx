#load "../utils.fsx"
open Utils

let input = GetData 24
let example = GetExample 24 1
example

// %%
let parseLine = function
    | Regex "(\d+)/(\d+)" [Int a; Int b] -> a,b

let parse input =
    input |> GetLines |> Array.map parseLine
    
parse example

// %%



// %%

let rec possibleBridges pins parts =
    let isMatch (p1, p2) = p1 = pins || p2 = pins
    [
        for part in (parts |> Array.filter isMatch) do
            let p1,p2 = part
            let remainingParts = parts |> Array.filter ((<>) part)
            let nextPins = if p1 = pins then p2 else p1
            yield (1, p1+p2)
            for (length, strength) in possibleBridges nextPins remainingParts do
                yield (length + 1, strength + p1+p2)
    ]
possibleBridges 0 [|(0, 2); (2, 2); (2, 3); (3, 4); (3, 5); (0, 1); (10, 1); (9, 10)|]

// %%
let calculateStrength (bridge:(int*int) list) =
    List.sumBy (fun (a,b) -> a+ b) bridge
// %%
let part1 input =
    parse input
    |> possibleBridges 0
    |> List.map snd
    |> List.max
part1 example
// %%
part1 input

// %%
let part2 input =
    parse input
    |> possibleBridges 0
    |> List.max
    |> snd
    
part2 example

// %%
part2 input
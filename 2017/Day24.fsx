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



let rec possibleMatches pins (parts: (int*int) array) =
    let isMatch (p1, p2) = p1 = pins || p2 = pins
    let matchingParts = parts |> Array.filter isMatch
    
    [
        for part in matchingParts do
            yield [part]
            let remainingParts = parts |> Array.except [part]
            let p1, p2 = part
            let nextPins = if p1 = pins then p2 else p1
            for bridge in possibleMatches nextPins remainingParts do
                yield part :: bridge
    ]        

possibleMatches 0 [|(0, 2); (2, 2); (2, 3); (3, 4); (3, 5); (0, 1); (10, 1); (9, 10)|]
// %%
let calculateStrength (bridge:(int*int) list) =
    List.sumBy (fun (a,b) -> a+ b) bridge
// %%
let part1 input =
    parse input
    |> possibleMatches 0
    |> List.map calculateStrength
    |> List.max
part1 example
// %%
part1 input

// %%
let part2 input =
    parse input
    |> possibleMatches 0
    |> List.groupBy List.length
    |> List.maxBy (fst)
    |> snd
    |> List.map calculateStrength
    |> List.max
    
part2 example

// %%
part2 input
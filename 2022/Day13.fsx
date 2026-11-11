// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 13 |> GetBlocks |> Array.map GetLines
input[..2]

// %%
let example =
    "[1,1,3,1,1]
    [1,1,5,1,1]

    [[1],[2,3,4]]
    [[1],4]

    [9]
    [[8,7,6]]

    [[4,4],4,4]
    [[4,4],4,4,4]

    [7,7,7,7]
    [7,7,7]

    []
    [3]

    [[[]]]
    [[]]

    [1,[2,[3,[4,[5,6,7]]]],8,9]
    [1,[2,[3,[4,[5,6,0]]]],8,9]"
    |> GetBlocks
    |> Array.map (GetLines >> Array.map (fun x -> x.Trim()))

example

// %%
type Packet =
    | OpenList
    | Digit of int
    | CloseList

let parseLine (s: string) =
    s.Replace("[", "[,").Replace("]", ",]") // just making parsing easier
    |> Split ","
    |> Seq.map (fun c -> 
        match c with 
        | "[" -> OpenList 
        | "]" -> CloseList 
        | _ -> Digit (c |> int))
    |> Seq.toList


let parseChunk (input:string array) = 
    parseLine input[0], parseLine input[1]

parseChunk input[2]

// %%
let rec compare (listA,listB) =
    //printfn $"{listA|>List.head}, {listB |> List.head}"
    match listA,listB with
    | a::restA, b::restB when a = b -> compare (restA, restB)
    // both values are integers, the lower integer should come first
    | Digit a::_, Digit b::_ when a < b -> true // If the left integer is lower than the right integer, the inputs are in the right order.
    | Digit a::_, Digit b::_ when a > b -> false // If the left integer is higher than the right integer, the inputs are not in the right order
    // If exactly one value is an integer
    | OpenList::_, Digit b::restB -> compare (listA,(OpenList::Digit b::CloseList::restB)) // convert the integer to a list which contains that integer as its only value, then retry the comparison
    | Digit a::restA, OpenList::_ -> compare ((OpenList::Digit a::CloseList::restA),listB) // convert the integer to a list which contains that integer as its only value, then retry the comparison
    // both values are lists
    | CloseList::_, _ -> true // If the left list runs out of items first, the inputs are in the right order
    | _, CloseList::_ -> false // If the right list runs out of items first, the inputs are not in the right order    

example
|> Array.map parseChunk
|> Array.indexed
|> Array.filter (fun (_, lists) -> compare lists)
|> Array.map (fst >> (+) 1)
|> Array.sum

// %%
input
|> Array.map parseChunk
|> Array.indexed
|> Array.filter (fun (_, lists) -> compare lists)
|> Array.map (fst >> (+) 1)
|> Array.sum

// %%
[|"[[2]]";"[[6]]";|] |> parseChunk

// %%
let sorted = 
    input
    |> Seq.map parseChunk
    |> Seq.collect (fun (a,b) -> [|a;b|])
    |> Seq.append ([|"[[2]]";"[[6]]";|] |> parseChunk |> fun (a,b) -> [|a;b|])
    |> Seq.indexed
    |> Seq.sortWith (fun (_,a) (_,b) -> if compare (a,b) then -1 else 1)
    |> Seq.toArray

[ 
    sorted |> Seq.findIndex (fun (i, _) -> i = 0)
    sorted |> Seq.findIndex (fun (i, _) -> i = 1)
] 
|> Seq.map ((+)1)
|> Seq.reduce (*)

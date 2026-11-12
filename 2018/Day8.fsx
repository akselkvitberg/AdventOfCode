#load "../utils.fsx"
open Utils

let input = GetData 8
let example = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2"
example

// %%
let parse input = input |> Split " " |> Array.map int |> List.ofArray
parse example

// %%
type Node = { Children: Node list; Metadata: int list }

let buildTree buffer =
    let rec buildTreeImpl (buffer: int list) : Node*int list =
        let childCount::metadataCount::rest = buffer

        let (children, rest) = [1..childCount] |> List.mapFold (fun buffer i -> buildTreeImpl buffer) rest
        let (metadata, rest) = List.splitAt metadataCount rest
        
        {
            Children = children
            Metadata = metadata
        },rest
    buildTreeImpl buffer |> fst
    
example |> parse |> buildTree

// %%
let part1 input =
    
    let rec sumNodes node = List.sum node.Metadata + List.sumBy sumNodes node.Children
    
    parse input
    |> buildTree
    |> sumNodes
    
part1 example
// %%
part1 input

// %%
let part2 input =
    
    let rec sumNodes node =
        if node.Children = []
        then List.sum node.Metadata
        else node.Metadata |> List.choose (fun m -> List.tryItem (m-1) node.Children) |> List.sumBy sumNodes
    
    parse input
    |> buildTree
    |> sumNodes
    
part2 example

// %%
part2 input
#load "../utils.fsx"
open Utils

let input = GetData 12
let example = GetExample 12 1
input

// %%
let setToString plants pos =
    System.String [| for i in pos - 2 .. pos + 2 -> if Set.contains i plants then '#' else '.' |]

let stringToSet str =
    set [ for i, c in Seq.indexed str do if c = '#' then i ]

let parseRule line =
    let [|state; output|] = Split " => " line
    if output = "#" then Some state else None
    
let parse input =
    let [|initial; rules|] = GetBlocks input
    let initial = (Split ": " initial)[1] |> stringToSet
    let rules = rules |> GetLines |> Array.choose parseRule |> Set.ofArray

    initial, rules
    
parse example

// %%

let nextGeneration rules plants =
    set [ for pos in Set.minElement plants - 2 .. Set.maxElement plants + 2 do
              let pattern = setToString plants pos
              if Set.contains pattern rules then pos ]

let part1 input =
    let plants, rules = parse input
    Seq.fold (fun plants _ -> nextGeneration rules plants) plants (seq {1..20})
    |> Seq.sum
part1 example
// %%
part1 input

// %%
let normalize plants =
    let first = Set.minElement plants
    first, Set.map (fun p -> p - first) plants

    

let part2 input =
    let plants, rules = parse input
    
    let rec run gen plants =
        let next = nextGeneration rules plants
        let first, shape = normalize plants
        let nextFirst, nextShape = normalize next
        if shape = nextShape then
            let remaining = 50_000_000_000L - int64 gen
            int64 (Seq.sum plants) + remaining * int64 (nextFirst - first) * int64 plants.Count
        else run (gen + 1) next
    
    run 0 plants |> string
    
part2 input
    
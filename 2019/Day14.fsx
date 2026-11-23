#load "../utils.fsx"
open Utils

let input = GetData 14
let examples = [ for i in 1..5 do GetExample 14 i ]
examples

// %%

type Chemical = string * int64
type ReactionMap = Map<string, (int64 * Chemical array)>

let parseChemical chemical: Chemical =
    let [|Long count; chemical|] = Split " " chemical
    chemical, count

let parseInputs input =
    input
    |> Split ","
    |> Array.map parseChemical

let parseLine line =
    let [|inputs; output|] =
        line
        |> Split "=>"
    let name, size = parseChemical output
    name, (size, parseInputs inputs)

let parse input : ReactionMap =
    GetLines input
    |> Array.map parseLine
    |> Map.ofArray
    
parse examples[2]

// %%
parse examples[0] |> Map.find "FUEL"

// %%
type Needs = Map<string, int64>
let need chemical needs =
    needs |> Map.tryFind chemical |> Option.defaultValue 0L

let addNeed chemical amount needs : Needs =
    let amountNeeded = (need chemical needs + amount) 
    Map.add chemical amountNeeded needs

let tryFindNext needs : string option =
    Map.tryFindKey (fun chemical amount -> chemical <> "ORE" && amount > 0L) needs

let produce (reactions: ReactionMap) (needs:Needs) chemical : Needs =
    let (size, inputs) = reactions[chemical]
    let amount = needs[chemical]
    let batches = (amount + size - 1L) / size
    let needs = addNeed chemical (-batches * size) needs
    
    inputs
    |> Array.fold (fun needs (ch, prBatch) -> addNeed ch (batches * prBatch) needs) needs

// %%
let rec oreNeeded reactions (needs: Needs) =
    match tryFindNext needs with
    | None -> needs["ORE"] // complete
    | Some chemical ->
        let needs = produce reactions needs chemical
        oreNeeded reactions needs
    

// %%
let part1 input =
    let reactions = parse input
    oreNeeded reactions (Map [("FUEL", 1L)])
part1 examples[0]

// %%
part1 input

// %%
let part2 input =
    let reactions = parse input
    let maxOre = 1_000_000_000_000L
    let oreMatch fuel  = oreNeeded reactions (Map [("FUEL", fuel)])
    
    let rec binarySearch (high:int64) (low:int64) =
        if high - low <= 1 then low
        else
            let mid = (low + high) / 2L
            let requiredOre = oreMatch mid
            if requiredOre <= maxOre then binarySearch high mid
            else binarySearch mid low
    binarySearch maxOre 0

part2 examples[2]
        
// %%
part2 input

// %%

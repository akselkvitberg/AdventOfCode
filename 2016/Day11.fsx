// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 11 |> GetLines

input[..5]

// %%
type Item = 
    | Generator of string
    | Microchip of string

let isValidState (items: Set<Item>) =
    let hasGenerator t = Set.contains (Generator t) items
    let noGenerators = Set.forall (function | Microchip _ -> true | _ -> false)
    let allMicrochipsMatch = Set.forall (function | Microchip m -> hasGenerator m | _ -> true) 
    noGenerators items || allMicrochipsMatch items

isValidState (Set.ofList [Generator "hydrogen"; Microchip "hydrogen"]),
isValidState (Set.ofList [Generator "helium"; Microchip "hydrogen"]),
isValidState (Set.ofList [Generator "hydrogen"; Microchip "hydrogen"; Generator "helium"; Microchip "helium"]),
isValidState (Set.ofList [Microchip "hydrogen"; Microchip "helium"]),
isValidState (Set.ofList [Generator "hydrogen"; Generator "helium";])

type State = 
    {
        Floor: int
        Items: Set<Item> array
        Moves: int
        PreviousState: State option
    } 
    member this.Hash = 
        sprintf "%d %A" this.Floor (this.Items |> Array.map (Set.toList >> List.sort >> List.map (function Generator _ -> "G" | Microchip _ -> "M") >> String.concat ""))
    member this.IsValid = this.Items |> Array.forall isValidState
        
let example = {
    Floor = 0
    Items = [|
        Set.ofList [Microchip "hydrogen"; Microchip "lithium"]
        Set.ofList [Generator "hydrogen"]
        Set.ofList [Generator "lithium"]
        Set.empty
    |]
    Moves = 0
    PreviousState = None
}
example

// %%
let parse (input:string array) = 
    let items = 
        input
        |> Array.mapi (fun i line ->
            let floor = i + 1
            let generators = line |> Matches "(\w+) generator" |> List.map (fun x -> Generator x)
            let microchips = line |> Matches "(\w+)-compatible microchip" |> List.map (fun x -> Microchip x)
            List.append generators microchips |> Set.ofList)
    {
        Floor = 0
        Items = items
        Moves = 0
        PreviousState = None
    }

parse input

// %%
let move (state:State) direction items =
    let ix = state.Floor + direction
    if ix < 0 || ix > 3 then
        state // Invalid move, return the same state
    else
        let fromFloor = Set.difference state.Items[state.Floor] items
        let toFloor = Set.union state.Items[ix] items
        let floors = state.Items |> Array.mapi (fun i f ->
            if i = state.Floor then fromFloor
            elif i = ix then toFloor
            else f)
        {
            Floor = ix
            Items = floors
            Moves = state.Moves + 1
            PreviousState = Some state
        }

move example 1 (Set.ofList [Microchip "hydrogen"])

// %%
type Queue<'a> = 'a list * 'a list
let emptyQueue = [],[]
let enqueue item ((fs, bs):Queue<'a>) = item::fs, bs
let enqueueMany items queue = List.fold (fun acc item -> enqueue item acc) queue items
let rec dequeue queue =
    match queue with
    | [],[] -> None, ([],[])
    | fs, [] -> dequeue ([],List.rev fs)
    | fs, b::bs -> Some b, (fs, bs)

emptyQueue |> enqueueMany [1;2;3] |> dequeue

// %%
let isSolution (state:State) =
    state.Items[0].Count = 0 &&
    state.Items[1].Count = 0 &&
    state.Items[2].Count = 0

// %%
let getValidStates (state:State) =
    let items = state.Items[state.Floor] |> Set.toList
    let pairs = items |> List.allPairs items |> List.map (fun (a,b) -> Set.ofList [a;b])
    
    [
        for pair in pairs do
        for direction in [-1; 1] do
            let newState = move state direction pair
            if newState.IsValid && state <> newState then
                yield newState
    ]
    
example |> getValidStates

// %%
let solve state =     
    let rec search queue seenStates =
        let (state, queue) = dequeue queue
        match state with
        | None -> failwith "No solution found"
        | Some s when isSolution s -> s
        | Some s when Set.contains s.Hash seenStates -> search queue seenStates
        | Some state -> 
            let nextStates = getValidStates state
            let newSeenStates = Set.add state.Hash seenStates
            let newQueue = enqueueMany nextStates queue
            search newQueue newSeenStates

    search (enqueue state emptyQueue) (Set.empty)

solve example

// %%
parse input
|> solve

// %%
let addPart2Modules (state:State) =
    let newItems = 
        [ "elerium"; "dilithium" ]
        |> List.collect (fun x -> [Generator x; Microchip x])
        |> Set.ofList
    let newFloors = state.Items |> Array.mapi (fun i f ->
        if i = 0 then Set.union f newItems
        else f)
    { state with Items = newFloors }
parse input
|> addPart2Modules
|> solve

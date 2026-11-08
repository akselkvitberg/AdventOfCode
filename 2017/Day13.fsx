// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 13
let example = GetExample 13 1
example

// %%
type State = {
    Time: int
    Position: int
    Scanners: Map<int, int array>
    Severity: Map<int, int>
}

// %%
let parse input =
    let scanners = 
        input
        |> GetLines
        |> Array.map (Split ":" >> Array.map int >> fun arr -> (arr[0], arr[1]))
        |> Map.ofArray
        
    {
        Time = 0
        Position = 0
        Scanners = scanners |> Map.map (fun _ depth -> [0..depth - 1] @ [depth - 2 .. -1 .. 1] |> Array.ofList)
        Severity = scanners
    }

parse example

// %%
let getSeverity (state:State) = 
    Map.tryFind state.Position state.Scanners
    |> Option.map (fun scannerPositions -> scannerPositions[state.Time % scannerPositions.Length])
    |> function 
        | Some 0 -> Some (state.Position * state.Severity[state.Position])
        | _ -> None
        
parse example
|> getSeverity

// %%
let calculateSeverity state =
    let maxLayer = state.Severity |> Map.keys |> Seq.max
    [0 .. maxLayer]
    |> List.choose (fun time -> getSeverity { state with Time = state.Time + time; Position = time })
    |> function
        | [] -> None
        | severities -> Some (severities |> List.sum)
    
parse example
|> calculateSeverity

// %%
parse input
|> calculateSeverity

// %%
let getTimeDelay state =
    [0..10000000]
    |> List.find (fun delay -> (calculateSeverity { state with Time = delay }) = None)

parse example
|> getTimeDelay

// %%
parse input
|> getTimeDelay

// There should be a better way to do this, but for now this works.
// Perhaps we can use gcd/lcm? Chinese Remainder Theorem? Modulus?

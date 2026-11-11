// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 15

// %%
let example = GetExample 15 1
example

// %%
let HASH step = step |> Seq.map int |> Seq.fold (fun acc i -> ((acc + i) * 17) % 256) 0
HASH "rn=1"

// %%
let part1 input =
    input |> Split "," |> Array.sumBy HASH

part1 example

// %%
part1 input

// %%
type Instruction = SetFocalLength of int | Remove
type Lens = Lens of string*int

let parseStep step = 
    match step with
    | Regex "(\w+)-" [label] -> label, Remove
    | Regex "(\w+)=(\d)" [label; Int focalLength] -> label, SetFocalLength(focalLength)
    | _ -> failwith $"bad step: {step}"

let setFocalLength box label focalLength =
    match box |> List.tryFindIndex (fun (Lens(l,_)) -> l = label) with
    | None -> Lens(label, focalLength) :: box
    | Some index -> box |> List.mapi (fun i x -> if i = index then Lens(label, focalLength) else x )

let processBox label instruction (box:Lens list) =
    match instruction with 
    | Remove -> box |> List.where (fun (Lens(l, _)) -> l <> label)
    | SetFocalLength(focalLength) -> setFocalLength box label focalLength

let processStep boxes step = 
    let label, instruction = parseStep step
    let boxId = HASH label
    let boxes' = Map.change boxId (function | None -> Some (processBox label instruction []) | Some box -> Some(processBox label instruction box)) boxes

    boxes'

example |> Split "," |> Array.fold processStep Map.empty

// %%
let calculateFocusingPower boxId lenses =
    let lensPower = lenses |> Seq.rev |> Seq.mapi (fun i (Lens(_, power)) -> (boxId + 1) * (i+1) * power)
    lensPower

let part2 input = 
    let boxes = input |> Split "," |> Array.fold processStep Map.empty
    boxes
    |> Seq.collect (fun x -> calculateFocusingPower x.Key x.Value)
    |> Seq.sum

part2 example

// %%
part2 input

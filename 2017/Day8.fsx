// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 8
let example = GetExample 8 1
example

// %%
let createOperation reg1 op value reg2 check comp registers = 
    let compare first = 
        match check with
        | ">" -> first > comp;
        | ">=" -> first >= comp
        | "<" -> first < comp
        | "<=" -> first <= comp
        | "==" -> first = comp
        | "!=" -> first <> comp
        | _ -> failwith "invalid operation"

    let op v = 
        let v = Option.defaultValue 0 v
        match op with 
        | "inc" -> Some (v + value)
        | "dec" -> Some (v - value)
        | _ -> Some (0)

    let reg2Value = 
        Map.tryFind reg2 registers
        |> Option.defaultValue 0

    if (compare reg2Value)
    then Map.change reg1 (fun v -> op v) registers
    else registers
    

let parseInstruction line =
    match line with
    | Regex "(\w+) (\w+) ([-\d]+) if (\w+) ([><!=]+) ([-\d]+)" [reg1; op; Int value; reg2; check; Int comp ] -> 
        createOperation reg1 op value reg2 check comp
    | _ -> failwith $"failed to parse {line}"

let part1 input =
    input
    |> GetLines
    |> Array.map parseInstruction
    |> Array.fold (fun acc i -> acc |> i ) Map.empty
    |> Map.toSeq
    |> Seq.maxBy snd

part1 example

// %%
part1 input

// %%
let part2 input =
    input
    |> GetLines
    |> Array.map parseInstruction
    |> Array.scan (fun acc i -> acc |> i ) Map.empty
    |> Array.map (Map.toArray)
    |> Array.filter (fun x -> not Array.isEmpty x)
    |> Array.map (Array.maxBy snd)
    |> Array.maxBy snd
    
part2 example

// %%
part2 input

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 16 |> GetLines

// %%
let Parse line = [| for m in RegularExpressions.Regex.Matches(line, @"(\w+): (\d+)") -> (m.Groups[1].Value, int m.Groups[2].Value) |]

let aunts = input |> Array.map Parse
aunts[0..5]

// %%
let mfcsam = Map.ofList [
        ("children", 3)
        ("cats", 7)
        ("samoyeds", 2)
        ("pomeranians", 3)
        ("akitas", 0)
        ("vizslas", 0)
        ("goldfish", 5)
        ("trees", 3)
        ("cars", 2)
        ("perfumes", 1)
    ]

// %%
let IsAuntMatch forensic (aunt:(string*int) array) = 
    aunt 
    |> Seq.map forensic
    |> Seq.reduce (&&)
    
let ModernForensics (prop, count) = mfcsam[prop] = count

let RunForensics forensicMethod aunts =
    (aunts |> Seq.indexed |> Seq.find (fun (i, aunt) -> IsAuntMatch ModernForensics aunt) |> fst ) + 1 //0 indexed

// %%
aunts |> RunForensics ModernForensics

// %%
let Retroencabulator (prop, count) =
    match mfcsam[prop] with
    | k when prop = "cats" || prop = "trees" -> k < count
    | k when prop = "pomeranians" || prop = "goldfish" -> k > count
    | k -> k = count

aunts |> RunForensics Retroencabulator

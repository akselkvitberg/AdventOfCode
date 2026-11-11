// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 19 |> GetLines

// %%
let reps = input[..^2]
let molecule = input[^0]

// %%
let ParseReplacement (line:string) =
    let [|source;_;replacement|] = line.Split(" ")
    (source, replacement)

let replacements = 
    reps 
    |> Seq.map ParseReplacement 
    //|> Seq.map (fun (key, values) -> (key, Seq.map snd values)) // Why is there no Seq.groupBy fst snd, which is equivalent of Linq .GroupBy(x => x.Item1, x => x.Item2)
replacements

// %%
let ReplaceAtom molecule atom replacement =
    molecule

let Replace molecule replacementGroup =
    0

// %%
let Replace (line:string) (replacement:string) index length =
    line[..index-1] + replacement + line[index + length..]

let rec GetReplacements (line:string) (startIndex:int) ((source:string), replacement) = 
    match line.IndexOf(source, startIndex) with
    | -1 -> []
    | index -> 
        let modp = Replace line replacement index source.Length
        let mods = GetReplacements line (index + 1) (source, replacement) 
        modp::mods

GetReplacements "ab ab ba ab ab" 0 ("ab", "x")

// %%
replacements
|> Seq.collect (GetReplacements molecule 0)
|> Seq.distinct
|> Seq.length

// %%
let reverseMap = replacements |> Seq.map (fun (key,v) -> (v,key)) |> Seq.sortBy (fun (x, b)-> -x.Length)

let Step (molecule, count) = 
    reverseMap 
        |> Seq.fold (fun (acc:string, count) (src, trg) -> 
        if(acc.Contains(src)) 
        then 
            let regex = new RegularExpressions.Regex(src)
            (regex.Replace(acc, trg, 1), count + 1)
        else (acc, count))
        (molecule, count)

(molecule, 0)
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step
|> Step

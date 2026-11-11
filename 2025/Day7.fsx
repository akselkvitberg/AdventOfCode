// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 7
let example = GetExample 7 1
example

// %%
let part1 input = 
    input
    |> GetLines
    |> Array.fold (fun (splits, beams: Set<int>) (line:string) -> 
        line
        |> Seq.indexed
        |> Seq.fold (fun (splits, beams: Set<int>) (i,c) -> 
            match c with
            | 'S' -> 0, Set.singleton i
            | '^' when Set.contains i beams -> splits+1, beams |> Set.add (i + 1) |> Set.add (i - 1) |> Set.remove i
            | _ -> splits, beams
            ) (splits, beams)
    ) (0, Set.empty)
    |> fst

part1 example

// %%
part1 input

// %%
let part2 input = 
    input
    |> GetLines
    |> Array.fold (fun (beams: Map<int, int64>) (line:string) -> 
        line
        |> Seq.indexed
        |> Seq.fold (fun (beams: Map<int, int64>) (i,c) -> 
            match c with
            | 'S' -> Map.ofSeq [(i, 1L)]
            | '^' when Map.containsKey i beams -> 
                let beamsAtI = beams[i]
                beams
                |> Map.change (i - 1) (function Some v -> Some (v + beamsAtI) | None -> Some beamsAtI)
                |> Map.change (i + 1) (function Some v -> Some (v + beamsAtI) | None -> Some beamsAtI)
                |> Map.remove i
            | _ -> beams
            ) beams
    ) Map.empty
    |> Map.values
    |> Seq.sum

part2 example

// %%
part2 input

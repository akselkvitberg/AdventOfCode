#load "../utils.fsx"
open System
open Utils

let input = GetData 8 |> Seq.map (string >> int) |> Seq.toArray
input

// %%
let layerSize = 25*6

input
|> Seq.chunkBySize (layerSize)
|> Seq.map (Array.countBy id >> Map.ofSeq)
|> Seq.minBy (fun c -> c[0])
|> fun d -> d[1] * d[2]

// %%
let layers = 
    input
    |> Array.chunkBySize (layerSize)
    |> array2D

Array.init layerSize (fun i -> layers[*,i] |> Array.find (fun v -> v < 2))
|> Array.chunkBySize 25
|> Array.map (Array.map (function | 0 -> ' ' | _ -> '#') >> String)
|> String.concat "\n"

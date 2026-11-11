#load "../utils.fsx"
open System
open Utils

let input = GetData 17 |> int
input

// %%
let runIteration steps ((buffer:int array), currentPosition) =
    let currentValue = buffer[currentPosition]
    let nextPos = (currentPosition + steps) % buffer.Length
    Array.insertAt (nextPos + 1) (currentValue+1) buffer, (nextPos + 1)

runIteration 3 ([|0|], 0)
|> runIteration 3
|> runIteration 3

// %%
let run steps iterations =
    Seq.init iterations id
    |> Seq.fold (fun state i -> runIteration steps state ) ([|0|],0)

run 3 2017

// %%
let part1 steps =
    let (buffer, pos) = run steps 2017
    buffer[pos+1]
    
part1 3
    

// %%
part1 input

// %%
let part2 steps =
    ((0, 0), [1 .. 50_000_000])                                                                   
      ||> List.fold (fun (pos, afterZero) value ->                                                  
          let next = (pos + steps) % value + 1      // value = current buffer length                
          next, (if next = 1 then value else afterZero))                                            
      |> snd

part2 3

// %%
part2 input
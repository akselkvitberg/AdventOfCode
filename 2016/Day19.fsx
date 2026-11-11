// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
let input = 3014603

// %%
let getElvesAndLeft count =
    Seq.init count (fun n -> n+1, (n+1) % count + 1)
    |> Map.ofSeq

getElvesAndLeft 5

// %%
let rec eliminateLeft (elves:Map<int,int>) index =
    let nextElf = elves[index]
    if nextElf = index then index
    else
        eliminateLeft (elves |> Map.add index elves[nextElf] |> Map.remove nextElf) elves[nextElf]

eliminateLeft (getElvesAndLeft 5) 1

// %%
eliminateLeft (getElvesAndLeft input) 1

// %%
open System.Collections.Generic

let removeAcross input =
    let que1 = LinkedList<int>([1..input/2])
    let que2 = LinkedList<int>([input/2+1..input])

    while que1.Count + que2.Count <> 1 do
        let x = que1.First.Value
        que1.RemoveFirst() |> ignore

        if que1.Count = que2.Count then
            que1.RemoveLast()
        else
            que2.RemoveFirst()
        
        que2.AddLast(x) |> ignore
        
        let a = que2.First.Value
        que2.RemoveFirst() |> ignore
        que1.AddLast(a) |> ignore

    printfn "%d" (que1.First.Value)

removeAcross 5

// %%
removeAcross input

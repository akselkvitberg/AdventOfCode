// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 20 |> int

// %%
let CountPresents houseNr =
    houseNr::[1..houseNr / 2]
    |> Seq.filter (fun i -> houseNr % i = 0)
    |> Seq.sumBy (fun i -> i * 10)

CountPresents 1000

// %%
Seq.initInfinite (fun i -> (i, CountPresents i))
|> Seq.skip (input / 40)
|> Seq.find (fun (i, presents) -> presents >= input / 1)

// %%
[1..8000]
|> Seq.sum
|> printfn "%i" 

printfn "%i" input

// %%
Seq.initInfinite (fun i -> (i, CountPresents i))
|> Seq.skip 100
|> Seq.map (fun (i, p) -> (i, p, i % 2 = 0, i % 3 = 0, i % 5 = 0, i % 7 = 0))
|> Seq.take 100

// %%
Seq.initInfinite id
|> Seq.skip 1
|> Seq.filter (fun i -> i % 2 = 0)
|> Seq.filter (fun i -> i % 3 = 0)
|> Seq.filter (fun i -> i % 5 = 0)
|> Seq.filter (fun i -> i % 7 = 0)
|> Seq.filter (fun i -> i % 11 = 0)
//|> Seq.filter (fun i -> i % 13 = 0) // too high
|> Seq.map (fun i -> (i, CountPresents i))
|> Seq.find (fun (i, presents ) -> presents >= input)

// AAAAaaahhhh!!
// %%
let count = 50
let nr = 1000
let i = 3
i * count
nr / i

// %%
let CountPresentsLazyElves houseNr =
    houseNr::[1..houseNr / 2]
    |> Seq.filter (fun i -> houseNr % i = 0)
    |> Seq.filter (fun i -> houseNr / i <= 50)
    |> Seq.sumBy (fun i -> i * 11)

let test = 786240
CountPresents test, CountPresentsLazyElves test

// %%
Seq.initInfinite id
|> Seq.skip 1
|> Seq.filter (fun i -> i % 2 = 0)
|> Seq.filter (fun i -> i % 3 = 0)
|> Seq.filter (fun i -> i % 5 = 0)
|> Seq.filter (fun i -> i % 7 = 0)
//|> Seq.filter (fun i -> i % 11 = 0)
|> Seq.map (fun i -> (i, CountPresentsLazyElves i))
|> Seq.take 100000
|> Seq.find (fun (i, presents ) -> presents >= input)

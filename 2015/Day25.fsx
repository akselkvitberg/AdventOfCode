// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
let row = 2978
let col = 3083

let firstCode = 20151125L

let code = (row + col - 1) * (row + col) / 2 - row + 1

code

// %%
let CalculateNext previous = (previous * 252533L) % 33554393L

// %%
[2..code]
|> Seq.fold (fun acc i -> CalculateNext acc) firstCode

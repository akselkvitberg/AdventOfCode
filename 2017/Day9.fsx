// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 9
input

// %%
let processInput input =

    let rec processGarbage garbageChars list =
        match list with 
        | [] -> garbageChars, []
        | '!'::_::rest -> processGarbage garbageChars rest
        | '>'::rest -> garbageChars, rest
        | _::rest -> processGarbage (garbageChars+1) rest

    let rec proc groupLevel groups garbageChars list =
        match list with 
        | [] -> groups, garbageChars
        | '!'::_::rest -> proc groupLevel groups garbageChars rest
        | '<'::rest ->    
            let garbageChars, rest = (processGarbage garbageChars rest)
            proc groupLevel groups garbageChars rest
        | '>'::rest ->    proc groupLevel groups garbageChars rest
        | '{'::rest ->    proc (groupLevel+1) (groups+groupLevel) garbageChars rest
        | '}'::rest ->    proc (groupLevel-1) groups garbageChars rest
        | x::rest ->      proc groupLevel groups garbageChars rest

    input
    |> List.ofSeq
    |> proc 1 0 0

processInput "{{<a!>},{<a!>},{<a!>},{<ab>}}"

// %%
processInput input

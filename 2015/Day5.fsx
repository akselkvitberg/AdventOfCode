// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 5 |> GetLines

// %%
let Vowels c = 
    match c with
    | 'a' -> true
    | 'e' -> true
    | 'i' -> true
    | 'o' -> true
    | 'u' -> true
    | _ -> false

let Forbidden a =
    match a with
    | ('a','b') -> true
    | ('c','d') -> true
    | ('p','q') -> true
    | ('x','y') -> true
    | _ -> false

let IsNice string = 
    let vowels = string |> Seq.filter Vowels |> Seq.length
    let doubleLetter = string |> Seq.pairwise |> Seq.exists (fun (a,b) -> a = b)
    let forbiddenChar = string |> Seq.pairwise |> Seq.exists Forbidden
    vowels >= 3 && doubleLetter && not forbiddenChar

IsNice "dvszwmarrgswjxmb"   

// %%
input |> Seq.filter IsNice |> Seq.length

// %%
let IsPair (i:int,pairs:seq<int * 'a * 'a>) =
    true

let HasPair line = 
    line |> Seq.pairwise 
         |> Seq.indexed 
         |> Seq.groupBy snd
         |> Seq.exists (fun (_, pairs) -> 
            let indexes = pairs |> Seq.map fst |> Seq.toArray
            indexes.Length >= 2 && abs (indexes[0] - indexes[1]) > 1
            )

HasPair "qjjhvhtzxzqqjkmpb"

// %%
let IsRepeat [|a;_;b|] =
    a = b

let HasRepeat string = 
    string |> Seq.windowed 3 |> Seq.exists IsRepeat

HasRepeat "abcbe"

// %%
let IsBetterNice string = 
    HasRepeat string && HasPair string

HasPair "qjhvhtzxzqqjkmpb"

// %%
input |> Seq.filter IsBetterNice |> Seq.length

// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils
let input = GetData 8 |> GetLines

// %%
let getEncodedStringLength (input:string) =
    System.Text.RegularExpressions.Regex.Unescape input[1..^1] |> String.length


let codedLength = input |> Seq.sumBy String.length
let unescapedLength = input |> Seq.sumBy getEncodedStringLength

codedLength - unescapedLength

// %%
open System.Text.RegularExpressions
let escapeText (input:string) =
    let escaped = Regex.Escape input
    let escaped2 = escaped |> Seq.filter (fun c -> c = '\"') |> Seq.length
    escaped.Length + escaped2 + 2

let escapedLength = input |> Seq.sumBy escapeText

escapedLength - codedLength

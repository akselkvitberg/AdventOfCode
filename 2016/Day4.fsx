// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 4 |> GetLines
input[..5]

// %%
let testData = [
    "aaaaa-bbb-z-y-x-123[abxyz]"
    "a-b-c-d-e-f-g-h-987[abcde]"
    "not-a-real-room-404[oarel]"
    "totally-real-room-200[decoy]"
]

// %%
let getRoomHash (roomName:string) =
    roomName.Replace("-", "")
    |> Seq.countBy id
    |> Seq.sortBy fst
    |> Seq.sortByDescending snd
    |> Seq.map fst
    |> Seq.take 5
    |> Seq.toArray
    |> fun x -> new string(x)
    

let parse =
    function
    | Regex "([a-z-]+)(\d+)\[(\w+)\]" [name; sector; checksum] when getRoomHash name = checksum -> Some(name, int sector)
    | _ -> None

testData 
|> Seq.choose parse
|> Seq.sumBy snd

// %%
input
|> Seq.choose parse
|> Seq.sumBy snd

// %%
let alphaStart = (int)'a'

let decryptChar (id:int) (c:char) =
    match c with
    | a when a = '-' -> ' '
    | a -> (char)((id - alphaStart + int c) % 26 + alphaStart)

decryptChar 26 'a'

// %%
let decrypt (input:string) id =
    input 
    |> Seq.map (decryptChar id)
    |> Seq.toArray
    |> fun x -> new string(x)
    

decrypt "qzmt-zixmtkozy-ivhz" 343

// %%
testData
|> Seq.choose parse
|> Seq.map (fun (name, id) -> decrypt name id)

// %%
input
|> Seq.choose parse
|> Seq.map (fun (name, id) -> decrypt name id, id)
|> Seq.find (fun (name, id) -> name.Contains("north"))

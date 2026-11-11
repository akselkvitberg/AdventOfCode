// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let lines = GetData 3 |> GetLines
let items = lines |> Seq.map Seq.toArray |> Seq.toArray
items[..3]

// %%
let arr = Array2D.init items.Length items[0].Length (fun i j -> items[i][j])

// %%
let isOne char = char = '1'

let countOnes (line: char[]) =
  line
  |> Seq.filter isOne
  |> Seq.length

// %%
let half = arr.GetLength(0) / 2

let byColumn column = arr[*, column] |> countOnes

let freq = [0..arr.GetLength(1)-1] |> Seq.map byColumn

let getMostCommon count = if count > half then '1' else '0'

let gamma = freq |> Seq.map getMostCommon |> Seq.toArray

let epsilon = gamma |> Seq.map (fun x -> if x = '1' then '0' else '1') |> Seq.toArray

gamma, epsilon

// %%
let gammaInt = Convert.ToInt32(new String(gamma), 2)
let epsilonInt = Convert.ToInt32(new String(epsilon), 2)
let result = gammaInt * epsilonInt

gammaInt, epsilonInt, result

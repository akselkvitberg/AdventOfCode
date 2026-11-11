// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 1 |> GetLines

input[..3]

// %%
let example1 = 
    """
    1abc2
    pqr3stu8vwx
    a1b2c3d4e5f
    treb7uchet
    """ |> GetLines

let example2 = 
    """
    two1nine
    eightwothree
    abcone2threexyz
    xtwone3four
    4nineeightseven2
    zoneight234
    7pqrstsixteen
    """ |> GetLines

// %%
let parse line = 
    match line with
    | Regex "(\d).*(\d)" [Int a; Int b] -> a * 10 + b
    | Regex "(\d)" [Int a] -> a * 10 + a
    | _ -> failwith "No number"

example1 |> Array.map parse |> Array.sum

// %%
input |> Array.map parse |> Array.sum

// %%
let parseNumber = 
    function
    | "one" -> 1
    | "two" -> 2
    | "three" -> 3
    | "four" -> 4
    | "five" -> 5
    | "six" -> 6
    | "seven" -> 7
    | "eight" -> 8
    | "nine" -> 9
    | Int a -> a
    | _ -> failwith "not a number"

let parseWords line =
    let captureGroup = "(one|two|three|four|five|six|seven|eight|nine|\d)"
    let regex = $"{captureGroup}.*{captureGroup}"
    match line with
    | Regex regex [a; b] -> (parseNumber a, parseNumber b) |> fun (a, b) -> a * 10 + b
    | Regex captureGroup [a] -> parseNumber a |> fun a -> a * 10 + a
    | _ -> failwith "No number"

example2 |> Array.map parseWords

// %%
input |> Array.map parseWords |> Array.sum

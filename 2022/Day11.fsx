// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 11 |> GetBlocks
input[0]

// %%
let example = 
    """Monkey 0:
    Starting items: 79, 98
    Operation: new = old * 19
    Test: divisible by 23
        If true: throw to monkey 2
        If false: throw to monkey 3

    Monkey 1:
    Starting items: 54, 65, 75, 74
    Operation: new = old + 6
    Test: divisible by 19
        If true: throw to monkey 2
        If false: throw to monkey 0

    Monkey 2:
    Starting items: 79, 60, 97
    Operation: new = old * old
    Test: divisible by 13
        If true: throw to monkey 1
        If false: throw to monkey 3

    Monkey 3:
    Starting items: 74
    Operation: new = old + 3
    Test: divisible by 17
        If true: throw to monkey 0
        If false: throw to monkey 1
    """
    |> GetBlocks
example[0]

// %%
type MonkeyId = int

type Monkey = {
    id: MonkeyId
    items: uint64 list
    operation: uint64 -> uint64
    divisibleBy: uint64
    testSucceedMonkey: MonkeyId
    testFailedMonkey: MonkeyId
    comparisons: uint64
}

// %%
let parse monkeyBlock =
    let [Int id] = monkeyBlock |> Match "Monkey (\d+):"
    let startingItems = monkeyBlock |> Matches "Starting items: (?:(\d+),? ?)+" |> (List.map uint64)
    let op = match monkeyBlock with 
             | Regex "old \* old" _ -> (fun x -> x * x)
             | Regex "old \* (\d+)" [ULong mult] -> (*) mult
             | Regex "old \+ (\d+)" [ULong term] -> (+) term
             | _ -> failwith monkeyBlock
    let [ULong test] = monkeyBlock |> Match "Test: divisible by (\d+)"
    let [Int testYes] = monkeyBlock |> Match "If true: throw to monkey (\d+)"
    let [Int testNo] = monkeyBlock |> Match "If false: throw to monkey (\d+)"
    let monkey = {
        id = id
        items = startingItems
        operation = op
        divisibleBy = test
        testSucceedMonkey = testYes
        testFailedMonkey = testNo
        comparisons = 0UL
    }
    monkey 

let parseInput input = input |> Seq.map parse |> Seq.map (fun m -> m.id, m) |> Map.ofSeq
let exampleMonkeys = parseInput example
exampleMonkeys

// %%
let inspect item monkey =
    let newWorryLevel = (monkey.operation item) / 3UL

    if newWorryLevel % monkey.divisibleBy = 0UL
    then (newWorryLevel, monkey.testSucceedMonkey)
    else (newWorryLevel, monkey.testFailedMonkey)

inspect exampleMonkeys[0].items[0] exampleMonkeys[0]

// %%
let rec processMonkey worryFunction (monkeys:Map<int, Monkey>) monkey = 
    match monkey.items with
    | [] -> monkeys
    | x::xs ->  
        let (value, throwTo) = worryFunction x monkey
        let newMonkey = {monkey with items = xs; comparisons = monkey.comparisons + 1UL}
        let newMap = monkeys 
                   |> Map.change throwTo (fun x -> match x with Some m -> Some {m with items = value::m.items} | None -> None)
                   |> Map.add monkey.id newMonkey
        processMonkey worryFunction newMap newMonkey

processMonkey inspect exampleMonkeys exampleMonkeys[1]

// %%
let processRound worryFunction (monkeys:Map<int,Monkey>) =
    monkeys.Keys
    |> Seq.fold (fun map monkey -> processMonkey worryFunction map map[monkey]) monkeys

processRound inspect exampleMonkeys

// %%
let processRounds worryFunction rounds monkeys =
    [1..rounds]
    |> Seq.fold (fun acc _ -> processRound worryFunction acc) monkeys

processRounds inspect 20 exampleMonkeys

// %%
let getMonkeyBusiness (monkeys:Map<int,Monkey>) =
    monkeys
    |> Map.values
    |> Seq.map (fun x -> x.comparisons)
    |> Seq.sortByDescending id
    |> Seq.take 2
    |> Seq.reduce (*)

exampleMonkeys
|> processRounds inspect 20
|> getMonkeyBusiness

// %%
input
|> parseInput
|> processRounds inspect 20
|> getMonkeyBusiness

// %%
// Solution shamelessly stolen from https://github.com/nooges/advent-of-code/blob/main/2022/day11.fsx
let getModulus monkeys =
    monkeys
    |> Map.values
    |> Seq.map (fun x -> x.divisibleBy)
    |> Seq.reduce (*)

getModulus exampleMonkeys

// %%
let inspectWorrysome modulus item monkey =
    let newWorryLevel = (monkey.operation item) % modulus

    if newWorryLevel % monkey.divisibleBy = 0UL
    then (newWorryLevel, monkey.testSucceedMonkey)
    else (newWorryLevel, monkey.testFailedMonkey)

inspectWorrysome (getModulus exampleMonkeys) exampleMonkeys[0].items[0] exampleMonkeys[0]

// %%
exampleMonkeys
|> processRounds (inspectWorrysome (getModulus exampleMonkeys)) 10000
|> getMonkeyBusiness

// %%
let monkeys = parseInput input
monkeys
|> processRounds (inspectWorrysome (getModulus monkeys)) 10000
|> getMonkeyBusiness

#load "../utils.fsx"
open Utils

let input = GetData 23
let example = ""
input

// %%
type Target = Value of int64 | Register of string

type Instruction =
    | Set of string * Target
    | Sub of string * Target
    | Mul of string * Target
    | JumpNz of Target * Target

let parseTarget = function
    | Long v -> Value v
    | r -> Register r

let parseLine line =
    match line with
    | Regex "set (.) (.+)" [r; v] -> Set (r, parseTarget v)
    | Regex "sub (.) (.+)" [r; v] -> Sub (r, parseTarget v)
    | Regex "mul (.) (.+)" [r; v] -> Mul (r, parseTarget v)
    | Regex "jnz (.) (.+)" [x; y] -> JumpNz (parseTarget x, parseTarget y)
    | line -> failwith line

let parse input = input |> GetLines |> Array.map parseLine

parse input

// %% Program state (from day 18, without the inbox)
type Program = {
    Registers: Map<string, int64>
    Pc: int
    MulCount: int
}

module Program =
    let init register = { Registers = register; Pc = 0; MulCount = 0 }
    let empty = init Map.empty

    let getValue program = function
        | Value v -> v
        | Register r -> Map.tryFind r program.Registers |> Option.defaultValue 0L

    let execute program (instructions: Instruction array) =
        let getValue = getValue program
        let register r = getValue (Register r)
        let instruction = instructions[program.Pc]
        match instruction with
        | Set (x, y) -> { program with Registers = Map.add x (getValue y) program.Registers; Pc = program.Pc + 1 }
        | Sub (x, y) -> { program with Registers = Map.add x (register x - getValue y) program.Registers; Pc = program.Pc + 1 }
        | Mul (x, y) -> { program with Registers = Map.add x (register x * getValue y) program.Registers; Pc = program.Pc + 1; MulCount = program.MulCount + 1 }
        | JumpNz (x, y) ->
            if getValue x <> 0L then { program with Pc = program.Pc + int (getValue y) }
            else { program with Pc = program.Pc + 1 }

// %%
let part1 input =
    let instructions = parse input
    
    let rec loop program =
        if program.Pc < 0 || program.Pc >= instructions.Length then program
        else
            loop (Program.execute program instructions)
    loop Program.empty
    
part1 input

// %%
[106500..17..123500]
// %% Part 2
// The program counts the numbers in b..17..c that are not prime.
// Lines 0-7 compute b and c; line 8 starts the slow prime check - (line 31: jnz 1 -23 = 8)
// So: run the program with a = 1 until it reaches line 8, read b and c, and count ourselves.

let sqrtInt64 = float >> sqrt >> int64

let hasFactor n =
    let largestDivisorToTry = sqrtInt64 n
    let divides d = n % d = 0L
    seq { 2L .. largestDivisorToTry } |> Seq.exists divides

let part2 input =
    let instructions = parse input

    let rec runSetup program =
        let program = Program.execute program instructions
        if program.Pc = 8 then program
        else runSetup program

    let registers = Map [("a", 1L)]
    let start = Program.init registers
    let setup = runSetup start
    let b = setup.Registers["b"]
    let c = setup.Registers["c"]

    [ b .. 17L .. c ] |> List.filter hasFactor |> List.length

part2 input

// %%

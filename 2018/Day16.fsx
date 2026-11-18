#load "../utils.fsx"
open System.Diagnostics.Tracing
open Utils

let input = GetData 16
input

// %%

type Instruction = int * (int * int * int) // opcode, (a, b, c)
type Sample = { Before: int list; Instruction: Instruction; After: int list }

let parseRegister = function Regex "\[(\d+), (\d+), (\d+), (\d+)" [Int a; Int b; Int c; Int d] -> [a;b;c;d]

let parseInstruction = function IntArray [| op; a; b; c |] -> op, (a, b, c)

let parseSample sample =
    let [|l1;l2;l3|] = GetLines sample
    {
        Before = parseRegister l1
        Instruction = parseInstruction l2
        After = parseRegister l3
    }

let parse input =
    let samples, program =
        match Split "\n\n\n\n" input with [| samples; program |] -> samples, program

    let samples =
        GetBlocks samples
        |> Array.map parseSample
        |> List.ofArray

    let program = GetLines program |> Array.map (parseInstruction) |> List.ofArray

    samples, program

parse input

// %%

type Opcode =
    | Addr | Addi | Mulr | Muli
    | Banr | Bani | Borr | Bori
    | Setr | Seti
    | Gtir | Gtri | Gtrr
    | Eqir | Eqri | Eqrr

let allOpcodes =
    [ Addr; Addi; Mulr; Muli
      Banr; Bani; Borr; Bori
      Setr; Seti
      Gtir; Gtri; Gtrr
      Eqir; Eqri; Eqrr ]

/// Runs one instruction and returns the new registers
let execute opcode (a, b, c) (regs: int list) =
    let r i = regs[i]
    
    let ofBool x = if x then 1 else 0
    let value =
        match opcode with
        | Addr -> r a + r b
        | Addi -> r a + b
        | Mulr -> r a * r b
        | Muli -> r a * b
        | Banr -> r a &&& r b
        | Bani -> r a &&& b
        | Borr -> r a ||| r b
        | Bori -> r a ||| b
        | Setr -> r a
        | Seti -> a
        | Gtir -> ofBool (a > r b)
        | Gtri -> ofBool (r a > b)
        | Gtrr -> ofBool (r a > r b)
        | Eqir -> ofBool (a = r b)
        | Eqri -> ofBool (r a = b)
        | Eqrr -> ofBool (r a = r b)
    List.updateAt c value regs

// %%
let matchesOpcode sample opcode =
    let variables = snd sample.Instruction
    let after = execute opcode variables sample.Before
    after = sample.After
    
let matchingOpcodes sample =
    allOpcodes |> List.filter (matchesOpcode sample)
// %%
let part1 input =
    let samples, program = parse input
    
    samples
    |> List.map matchingOpcodes
    |> List.filter (fun ops -> ops.Length >= 3)
    |> List.length

part1 input

// %%
let getMatchingOpcodes group =
    group
    |> List.map (matchingOpcodes >> Set.ofList)
    |> Set.intersectMany

let solveOpcodes samples =
    let possibilities =
        samples
        |> List.groupBy (fun s -> fst s.Instruction)
        |> List.map (fun (n, g) -> n, getMatchingOpcodes g)
        |> Map.ofList
    
    let rec solve solved possibilities =
        if Map.isEmpty possibilities then solved
        else
            let number, candidates = possibilities |> Map.toList |> List.find (fun (_, ops) -> Set.count ops = 1)
            let opcode = Set.minElement candidates
            let possibilities =
                possibilities
                |> Map.remove number
                |> Map.map (fun _ ops -> Set.remove opcode ops)
            let solved = Map.add number opcode solved
            solve solved possibilities
    
    solve Map.empty possibilities

let part2 input =
    let samples, program = parse input
    let opCodes = solveOpcodes samples
    let registers =
        program
        |> List.fold (fun regs (op, args) -> execute opCodes[op] args regs) [0;0;0;0]
    registers[0]
    
part2 input
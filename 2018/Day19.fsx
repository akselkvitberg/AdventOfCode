#load "../utils.fsx"
open Utils

let input = GetData 19
let example = GetExample 19 1
example

// %%
// Opcodes, copied from Day 16

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

let opcodeByName = allOpcodes |> List.map (fun op -> (string op).ToLower(), op) |> Map.ofList
// %%

let parseLine line =
    let [|op; Int a; Int b; Int c|] = Split " " line
    let opcode = opcodeByName[op]
    opcode, (a,b,c)

let parse input =
    let lines = GetLines input
    let ip = lines[0] |> Split " " |> fun x -> x[1] |> int
    let instructions = lines[1..] |> Array.map parseLine
    ip, instructions
parse example

// %%
/// Runs the program until the instruction pointer leaves it or `stopAt ip` is true,
/// and returns the registers
let runUntil stopAt ipRegister (instructions: (Opcode * (int * int * int))[]) registers =
    let rec loop ip registers =
        if ip < 0 || ip >= instructions.Length || stopAt ip then registers
        else
            let opcode, args = instructions[ip]
            let registers =
                registers
                |> List.updateAt ipRegister ip   // write the instruction pointer to its register
                |> execute opcode args
            loop (registers[ipRegister] + 1) registers  // read it back, then move to the next instruction
    loop 0 registers

/// Runs the program until the instruction pointer leaves it
let run = runUntil (fun _ -> false)

let part1 input =
    let (ip, instructions) = parse input
    let registers = run ip instructions [ 0; 0; 0; 0; 0; 0 ]
    registers[0]

part1 example

// %%
part1 input

// %%

let sumOfDivisors n = seq { 1 .. n } |> Seq.filter (fun d -> n % d = 0) |> Seq.sum

let part2 input =
    let (ip, instructions) = parse input
    let registers = runUntil ((=) 1) ip instructions [ 1; 0; 0; 0; 0; 0 ]
    sumOfDivisors registers[4]

// %%
part2 input
#load "../utils.fsx"
open Utils

let input = GetData 21
input

// %%
// Machine from Day 19

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
let execute opcode (a: int64, b: int64, c: int64) (regs: int64 list) =
    let r (i: int64) = regs[int i]

    let ofBool x = if x then 1L else 0L
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
    List.updateAt (int c) value regs

let opcodeByName = allOpcodes |> List.map (fun op -> (string op).ToLower(), op) |> Map.ofList

// %%

let parseLine line =
    let [| op; Long a; Long b; Long c |] = Split " " line
    let opcode = opcodeByName[op]
    opcode, (a, b, c)

let parse input =
    let lines = GetLines input
    let ip = lines[0] |> Split " " |> fun x -> x[1] |> int
    let instructions = lines[1..] |> Array.map parseLine
    ip, instructions

parse input

// %%
/// Runs the program until the instruction pointer leaves it or `stopAt ip` is true,
/// and returns the registers
let runUntil stopAt ipRegister (instructions: (Opcode * (int64 * int64 * int64))[]) registers =
    let rec loop ip registers =
        if ip < 0 || ip >= instructions.Length || stopAt ip then registers
        else
            let opcode, args = instructions[ip]
            let registers =
                registers
                |> List.updateAt ipRegister (int64 ip)   // write the instruction pointer to its register
                |> execute opcode args
            loop (int registers[ipRegister] + 1) registers  // read it back, then move to the next instruction
    loop 0 registers

// %%

// instruction 28: eqrr 3 0 5 - sets r5 = (r3 == r0). (1 or 0)
// instruction 29: adds r5 to r2 (which is instruction pointer) - in effect skipping a instruction - moving out of the program
// so earliest stop is if r0 = r3 at 28

let part1 input =
    let (ip, instructions) = parse input
    let registers = runUntil ((=) 28) ip instructions [ 0L; 0L; 0L; 0L; 0L; 0L ]
    registers[3]

part1 input

// %%
// cheat sheet
// addr a b c -> let rC = rA + rB
// addi a b c -> let rC = rA + b
// mulr a b c -> let rC = rA * rB
// muli a b c -> let rC = rA * b
// banr a b c -> let rC = rA &&& rB
// bani a b c -> let rC = rA &&& b
// borr a b c -> let rC = rA ||| rB
// bori a b c -> let rC = rA ||| b
// setr a _ c -> let rC = rA
// seti a _ c -> let rC = a
// gtir a b c -> let rC = if a > rB then 1 else 0
// gtri a b c -> let rC = if rA > b then 1 else 0
// gtrr a b c -> let rC = if rA > rB then 1 else 0
// eqir a b c -> let rC = if a = rB then 1 else 0
// eqri a b c -> let rC = if rA = b then 1 else 0
// eqrr a b c -> let rC = if rA = rB then 1 else 0
//
// When c is the ipRegister, jump
// seti X _ 2    go to X + 1
// addr N 2 2    if rN = 1 then skip the next instruction

let nextValue seed multiplier r3 =
    // instructions 8-27: the inner loop
    let rec innerLoop r3 r4 =
        let r5 = r4 &&& 255L         //  8  bani 4 255 5
        let r3 = r3 + r5             //  9  addr 3 5 3
        let r3 = r3 &&& 16777215L    // 10  bani 3 16777215 3
        let r3 = r3 * multiplier     // 11  muli 3 65899 3
        let r3 = r3 &&& 16777215L    // 12  bani 3 16777215 3
        if r4 < 256L then r3         // 13-16  if 256 > r4, go to 28 (the check)
        else
            let r4 = r4 / 256L       // 17-26  r4 = r4 / 256
            innerLoop r3 r4          // 27  go to 8

    // instructions 6-7: start of the outer loop
    let r4 = r3 ||| 65536L           //  6  bori 3 65536 4
    let r3 = seed                    //  7  seti 10649702 3 3
    innerLoop r3 r4

let part2 input =
    let _, instructions = parse input
    let _, (seed, _, _) = instructions[7]         // seti seed _ 3
    let _, (_, multiplier, _) = instructions[11]  // muli 3 multiplier 3

    let rec findLast seen previous r3 =
        let value = nextValue seed multiplier r3
        if Set.contains value seen then previous
        else findLast (Set.add value seen) value value

    findLast Set.empty 0L 0L   // r3 is 0 when the program first reaches instruction 6

part2 input

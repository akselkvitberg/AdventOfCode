#load "../utils.fsx"
open Utils

let input = GetData 18
let example = GetExample 18 1
example

// %%
type Target = Value of int64 | Register of string
type Part = Part1 | Part2

type Instructions =
    | Set of string * Target
    | Add of string * Target
    | Mul of string * Target
    | Mod of string * Target
    | Recover of Target
    | Sound of string
    | Send of Target
    | Receive of string
    | JumpGtz of Target * Target
    
let parseLine part line=
    match (part, line) with
    | Part1, Regex "snd (.)" [r] -> Sound r
    | Part1, Regex "rcv (.)" [r] -> Recover (Register r)
    | Part2, Regex "snd (.)" [Int r] -> Send (Value r)
    | Part2, Regex "snd (.)" [r] -> Send (Register r)
    | Part2, Regex "rcv (.)" [r] -> Receive r
    
    | _, Regex "add (.) (.+)" [r; Int v] -> Add (r, Value v)
    | _, Regex "add (.) (.+)" [r; v] -> Add (r, Register v)
    | _, Regex "set (.) (.+)" [r; Int v] -> Set (r,Value v)
    | _, Regex "set (.) (.+)" [r; v] -> Set (r,Register v)
    | _, Regex "mul (.) (.+)" [r; Int v] -> Mul (r,Value v)
    | _, Regex "mul (.) (.+)" [r; v] -> Mul (r,Register v)
    | _, Regex "mod (.) (.+)" [r; Int v] -> Mod (r,Value v)
    | _, Regex "mod (.) (.+)" [r; v] -> Mod (r,Register v)
    | _, Regex "jgz (.) (.+)" [Int r; Int v] -> JumpGtz (Value r,Value v)
    | _, Regex "jgz (.) (.+)" [Int r; v] -> JumpGtz (Value r,Register v)
    | _, Regex "jgz (.) (.+)" [r; Int v] -> JumpGtz (Register r,Value v)
    | _, Regex "jgz (.) (.+)" [r; v] -> JumpGtz (Register r,Register v)

    | _, line -> failwith line
    
let parse part input  = input |> GetLines |> Array.map (parseLine part)

let parse1 = parse Part1
let parse2 = parse Part2

parse1 input
parse2 input

// %%

type Program = {
    Registers: Map<string, int64>
    Pc: int
    Inbox: int64 list
}



module Program =
    let empty id = {Registers = Map ["p", id]; Pc = 0; Inbox = [];}
    
    let getValue program = function
        | Value v -> v
        | Register r -> Map.tryFind r program.Registers |> Option.defaultValue 0L
    
    let execute program instruction =
        let getValue = getValue program
        let newReg =
            match instruction with
            | Set(x, y) -> Map.add x (getValue y) program.Registers
            | Add(x, y) -> Map.change x (Option.map ((+) (getValue y))) program.Registers
            | Mul(x, y) -> Map.change x (Option.map ((*) (getValue y))) program.Registers
            | Mod(x, y) -> Map.change x (Option.map (fun v -> v % (getValue y))) program.Registers
            | Sound (x) -> Map.add "sound" (getValue (Register x)) program.Registers
        { program with Registers = newReg; Pc = program.Pc + 1 }

type ExecutionResult =
    | Continue of Program
    | Recovered of int64
    | Blocked
    | Sent of int64 * Program
    
let executeInstruction (program:Program) (instructions: Instructions array) =
    let instruction = instructions[program.Pc]
    let nextPc = program.Pc+1
    let getValue = Program.getValue program
        
    let jgz (x:Target) (y: Target) =
        if getValue x > 0
        then program.Pc + (getValue y |> int)
        else nextPc
        
    match instruction with
    | Recover (x) ->
        let value = getValue x
        let sound = getValue (Register "sound")
        if value <> 0 then Recovered sound
        else Continue { program with Pc = nextPc }
    | Receive x ->
        match program.Inbox with
        | [] -> Blocked
        | head::tails ->
            let registers = Map.add x head program.Registers
            Continue { program with Pc = nextPc; Inbox = tails; Registers = registers }
    | Send x ->
        Sent (getValue x, {program with Pc = nextPc})
    | JumpGtz (x, y) -> Continue {program with Pc = jgz x y}
    | _ -> Continue (Program.execute program instruction) 



// %%
let part1 input =
    let instructions = parse1 input
       
    let rec loop program =
        if program.Pc < 0 || program.Pc >= instructions.Length then None
        else
            match executeInstruction program instructions with
            | Recovered sound -> Some sound
            | Continue program -> loop program
    loop (Program.empty 0)
    |> Option.defaultValue 0
    |> string

part1 example
    

// %%
part1 input

// %% PART 2

let runUntilBlocked instructions program =
    let rec loop program sent =
        if program.Pc < 0 || program.Pc >= Array.length instructions then
            program, List.rev sent
        else
            match executeInstruction program instructions with
            | Blocked -> program, List.rev sent
            | Sent (value, program) -> loop program (value :: sent)
            | Continue program -> loop program sent
            | Recovered _ -> failwith "not in part 2"
    loop program []

let part2 input =
    let instructions = parse2 input
    let run = runUntilBlocked instructions
    let deliver messages program = { program with Inbox = program.Inbox @ messages }

    let rec loop p0 p1 sentByP1 =
        let p0, out0 = run p0
        let p1, out1 = run (deliver out0 p1)
        let sentByP1 = sentByP1 + out1.Length
        if out1.IsEmpty then sentByP1
        else loop (deliver out1 p0) p1 sentByP1
        
    loop (Program.empty 0) (Program.empty 1) 0

part2 input

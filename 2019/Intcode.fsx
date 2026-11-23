open System

type Machine =
    { Memory: Map<int64, int64>
      Pointer: int64
      RelativeBase: int64
      Inputs: int64 list }

type RunResult =
    | Output of int64 * Machine   // produced a value; continue with the machine
    | NeedsInput of Machine       // opcode 3 with no inputs left; add inputs and run again
    | Halted of Machine           // opcode 99; the machine is kept so memory can be inspected

let parse (text: string) =
    text.Split(',', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
    |> Array.map int64

let create (program: int64 array) =
    { Memory = program |> Array.mapi (fun i value -> int64 i, value) |> Map.ofArray
      Pointer = 0L
      RelativeBase = 0L
      Inputs = [] }

let addInputs inputs machine =
    { machine with Inputs = machine.Inputs @ inputs }

let read address machine =
    machine.Memory |> Map.tryFind address |> Option.defaultValue 0L

/// Run until the next output, a missing input, or halt
let rec run machine =
    let get address = read address machine
    let instruction = get machine.Pointer

    // parameter n's mode is digit n + 2 of the instruction (counted from the right, starting at 1)
    let mode n = instruction / pown 10L (n + 1) % 10L

    let address n =
        let p = machine.Pointer + int64 n
        match mode n with
        | 0L -> get p
        | 1L -> p
        | 2L -> machine.RelativeBase + get p
        | m -> failwith $"unknown parameter mode {m} at {machine.Pointer}"

    let param n = get (address n)
    let write n value = Map.add (address n) value machine.Memory
    let next size = machine.Pointer + int64 size

    match instruction % 100L with
    // 1 add: [p3] <- p1 + p2
    | 1L -> run { machine with Memory = write 3 (param 1 + param 2); Pointer = next 4 }
    // 2 multiply: [p3] <- p1 * p2
    | 2L -> run { machine with Memory = write 3 (param 1 * param 2); Pointer = next 4 }
    // 3 input: [p1] <- next input; pauses with NeedsInput when there is none
    | 3L ->
        match machine.Inputs with
        | [] -> NeedsInput machine
        | value :: rest -> run { machine with Memory = write 1 value; Pointer = next 2; Inputs = rest }
    // 4 output: emit p1
    | 4L -> Output (param 1, { machine with Pointer = next 2 })
    // 5 jump-if-true: if p1 <> 0 then jump to p2
    | 5L -> run { machine with Pointer = if param 1 <> 0L then param 2 else next 3 }
    // 6 jump-if-false: if p1 = 0 then jump to p2
    | 6L -> run { machine with Pointer = if param 1 = 0L then param 2 else next 3 }
    // 7 less than: [p3] <- 1 if p1 < p2, else 0
    | 7L -> run { machine with Memory = write 3 (if param 1 < param 2 then 1L else 0L); Pointer = next 4 }
    // 8 equals: [p3] <- 1 if p1 = p2, else 0
    | 8L -> run { machine with Memory = write 3 (if param 1 = param 2 then 1L else 0L); Pointer = next 4 }
    // 9 adjust relative base: relative base += p1
    | 9L -> run { machine with RelativeBase = machine.RelativeBase + param 1; Pointer = next 2 }
    // 99 halt
    | 99L -> Halted machine
    | op -> failwith $"unknown opcode {op} at {machine.Pointer}"

/// Run until the machine halts or needs input. Returns the outputs in order and how it stopped.
let runUntilBlocked machine =
    let rec loop outputs machine =
        match run machine with
        | Output (value, machine) -> loop (value :: outputs) machine
        | stopped -> List.rev outputs, stopped
    loop [] machine

/// Run a program with the given inputs to the end and return all outputs
let execute inputs program =
    create program |> addInputs inputs |> runUntilBlocked |> fst

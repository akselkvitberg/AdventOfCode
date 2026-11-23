#load "../utils.fsx"
open Utils

let input = GetData 2 |> Split "," |> Array.map int
input

// %%
let execute operation (program: int array) position =
    let i1 = program[position + 1]
    let i2 = program[position + 2]
    let i3 = program[position + 3]
    program[i3] <- operation program[i1] program[i2]

let run program =
    let program = Array.copy program
    let rec loop counter =
        match program[counter] with
        | 1 -> execute (+) program counter; loop (counter + 4)
        | 2 -> execute (*) program counter; loop (counter + 4)
        | 99 -> program[0]
        | op -> failwith $"unhandled opcode {op} at {counter}"
    loop 0
    
run [|1;9;10;3;2;3;11;0;99;30;40;50|]

// %%
let modifyAndRun program noun verb = 
    let modifiedProgram = Array.copy program
    modifiedProgram[1] <- noun
    modifiedProgram[2] <- verb
    run modifiedProgram

modifyAndRun input 12 2

// %%
[ for n in 0..99 do for v in 0..99 -> n, v ]
|> List.find (fun (n, v) -> modifyAndRun input n v = 19690720)
|> fun (n, v) -> n * 100 + v

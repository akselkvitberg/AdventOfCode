#load "../utils.fsx"
open Utils

let input = GetData 25
let example = GetExample 25 1
example

let exampleRule = """In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B."""
exampleRule

// %%

let parseHeader =
    function Regex "Begin in state (.)\.\s+Perform a diagnostic checksum after (\d+) steps" [state; Int steps] -> state, steps

type Rule = {
    Write: int
    Move: int
    NextState: string
}

let parseMove = function "left" -> -1 | "right" -> 1
let regex = @"In state (.):\s+If the current value is 0:\s+- Write the value (\d)\.\s+- Move one slot to the (\w+)\.\s+- Continue with state (.)\.\s+If the current value is 1:\s+- Write the value (\d)\.\s+- Move one slot to the (\w+)\.\s+- Continue with state (.)\."
let parseRule =
    function
        | Regex regex [s1; Int w1; m1; c1; Int w2; m2; c2] ->
            s1,
            [|
                {Write = w1; Move = parseMove m1; NextState = c1}
                {Write = w2; Move = parseMove m2; NextState = c2}
            |]
        | line -> failwith line
    
parseRule exampleRule
// %%
type Rules = Map<string, Rule array>
let parse input =
    let blocks = GetBlocks input
    let start, steps = blocks[0] |> parseHeader
    let rules = blocks[1..] |> Array.map parseRule |> Map.ofArray
    start, steps, rules
    
parse example

// %%
let turingMachineStep (rules: Rules) (tape,pos,state) =
    let currentValue = Map.tryFind pos tape |> Option.defaultValue 0
    let rulesForState = Map.find state rules
    let rule = rulesForState[currentValue]
    
    let tape = Map.add pos rule.Write tape
    let pos = pos + rule.Move
    let state = rule.NextState
    
    (tape, pos, state)

let runTuringMachine start steps rules =
    seq { 1..steps }
    |> Seq.fold (fun state _ -> turingMachineStep rules state) (Map.empty, 0, start)

// %%
let part1 input =
    let start, steps, rules = parse input
    
    runTuringMachine start steps rules
    |> (fun (tape, _, _) -> tape)
    |> Map.values
    |> Seq.filter ((=) 1)
    |> Seq.length
    
part1 example

// %%
part1 input
// Opened automatically by Polyglot Notebooks; needed when running as a plain script.
open System.IO
open System.Text

// %%
#load "../utils.fsx"
open Utils

let input = GetData 7 |> GetLines
let example = GetExample 7 1 |> GetLines
example

// %%
let parse input = 
    match input with
    | Regex @"Step (.) must be finished before step (.) can begin." [a; b] -> (a, b)
    | _ -> failwith "Invalid input"

Array.map parse example

// %%
let getStepOrder input =
    let requirements = Array.map parse input |> Array.toList
    let nodes = requirements |> List.collect (fun (a,b) -> [a; b]) |> List.distinct

    let dependencyMap = 
        requirements
        |> List.groupBy snd
        |> List.map (fun (node, reqs) -> node, reqs |> List.map fst |> Set.ofList)
        |> Map.ofList

    let getNextToProcess completed =
        nodes
        |> List.except completed
        |> List.filter (fun n -> 
            let reqs = Map.tryFind n dependencyMap |> Option.defaultValue Set.empty
            Set.isSubset reqs (Set.ofList completed)
        )
        |> List.sort
        |> List.head

    let rec buildOrder completed =
        if List.length completed = List.length nodes then
            completed
        else
            let next = getNextToProcess completed
            buildOrder (completed @ [next])

    buildOrder []
    |> String.concat ""

getStepOrder example

// %%
getStepOrder input

// %%
type Worker = { Step: string; CompletedTime: int }

let getConcurrentStepOrder input workerCount baseDuration =
    let requirements = Array.map parse input |> Array.toList
    let nodes = requirements |> List.collect (fun (a,b) -> [a; b]) |> List.distinct

    let dependencyMap = 
        requirements
        |> List.groupBy snd
        |> List.map (fun (node, reqs) -> node, reqs |> List.map fst |> Set.ofList)
        |> Map.ofList

    let getNextToProcess completed inProgress =
        nodes
        |> List.except completed
        |> List.except inProgress
        |> List.filter (fun n -> 
            let reqs = Map.tryFind n dependencyMap |> Option.defaultValue Set.empty
            Set.isSubset reqs (Set.ofList completed)
        )
        |> List.sort

    let getDuration (step:string) = baseDuration + (int step.[0] - int 'A' + 1)

    let rec processSteps workers completed time =
        // free the workers that are done, and mark their steps as completed
        let finished, busy = workers |> List.partition (fun w -> w.CompletedTime <= time)
        let completed = completed @ (finished |> List.map (fun w -> w.Step))

        if List.length completed = List.length nodes then
            time
        else
            // give available steps to the free workers
            let inProgress = busy |> List.map (fun w -> w.Step)
            let started =
                getNextToProcess completed inProgress
                |> List.truncate (workerCount - busy.Length)
                |> List.map (fun step -> { Step = step; CompletedTime = time + getDuration step })

            // jump to the next time a worker finishes
            let workers = busy @ started
            let nextTime = workers |> List.map (fun w -> w.CompletedTime) |> List.min
            processSteps workers completed nextTime

    processSteps [] [] 0

getConcurrentStepOrder example 2 0

// %%
getConcurrentStepOrder input 5 60

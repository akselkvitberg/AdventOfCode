#load "../utils.fsx"
open Utils

let input = GetData 15
input

// %%

type Kind = Elf | Goblin
type Unit = { Id: int; Pos: int*int; Hp: int; Kind: Kind }

let parse input =
    let grid = ToGrid input |> Array.map (fun ((x, y), c) -> (y, x), c)
    let walls = filterGrid '#' grid
    let units =
        [ for pos, c in grid do
              match c with
              | 'G' -> Goblin, pos
              | 'E' -> Elf, pos
              | _ -> () ]
        |> List.mapi (fun i (kind, pos) -> i, { Id = i; Pos = pos; Hp = 200; Kind = kind })
        |> Map.ofList

    walls, units

parse input
// %%
let emptyQueue = [],[]

let enqueue item = function fs, bs -> item::fs, bs

let rec dequeue queue =
    match queue with
    | [],[] -> None, ([],[])
    | fs, [] -> dequeue ([],List.rev fs)
    | fs, b::bs -> Some b, (fs, bs)

// in reading order
let getNeighbours (y, x) =
    [
        (y-1, x)
        (y, x-1)
        (y, x+1)
        (y+1, x)
    ]

let bfs start isBlocked isTarget =
    let rec loop queue visited =
        let item, queue = dequeue queue
        match item with
        | None -> visited
        | Some (node,_) when isTarget node -> visited
        | Some (node,cost) ->
            let next =
                getNeighbours node
                |> List.filter (fun n -> not (isBlocked n) && not (Map.containsKey n visited))
            let queue' = next |> List.fold (fun acc n -> enqueue (n, cost+1) acc) queue
            let visited' = next |> List.fold (fun acc n -> Map.add n (cost+1) acc) visited
            loop queue' visited'

    loop (emptyQueue |> enqueue (start, 0)) (Map.ofList [start, 0])

let closest dist candidates =
    candidates
    |> List.choose (fun p -> Map.tryFind p dist |> Option.map (fun d -> d, p))
    |> List.sort
    |> List.tryHead
    |> Option.map snd

// %%

let enemyToAttack enemies pos =
        enemies
        |> List.filter (fun e -> List.contains e.Pos (getNeighbours pos))
        |> List.sortBy (fun e -> e.Hp, e.Pos)
        |> List.tryHead

type Units = Map<int, Unit>
type TurnResult = NothingHappend of Units | UnitDied of Kind*Units

let takeTurn walls power units id =
    let unit = Map.find id units
    let enemies = Map.values units |> Seq.filter (fun u -> u.Kind <> unit.Kind) |> List.ofSeq
    let occupied = Map.values units |> Seq.map _.Pos |> Set.ofSeq
    let isBlocked p = Set.contains p walls || Set.contains p occupied
    let enemyToAttack = enemyToAttack enemies
    
    let pos =
        if (enemyToAttack unit.Pos).IsSome then unit.Pos else
        let inRange = enemies |> List.collect (fun e -> getNeighbours e.Pos)
        let steps = getNeighbours unit.Pos
        match closest (bfs unit.Pos isBlocked (fun p -> List.contains p inRange)) inRange with
        | None -> unit.Pos
        | Some target -> (closest (bfs target isBlocked (fun p -> List.contains p steps)) steps).Value

    let units = units |> Map.add id { unit with Pos = pos }
    match enemyToAttack pos with
    | None -> NothingHappend units
    | Some e when e.Hp <= power unit.Kind -> UnitDied (e.Kind, units |> Map.remove e.Id)
    | Some e -> units |> Map.add e.Id { e with Hp = e.Hp - power unit.Kind } |> NothingHappend

let combat stopWhenElfDies elfPower (walls, units: Map<int, Unit>) =
    let power kind = if kind = Elf then elfPower else 3
    let rec round n (units: Map<int, Unit>) =
        let order = units.Values |> Seq.sortBy _.Pos |> Seq.map _.Id |> List.ofSeq
        let rec turns (units: Map<int, Unit>) ids =
            match ids with
            | [] -> round (n + 1) units
            | id :: rest when not (units.ContainsKey id) -> turns units rest
            | id :: rest ->
                if units.Values |> Seq.forall (fun u -> u.Kind = units[id].Kind) then Some(n, units)
                else
                    match stopWhenElfDies, takeTurn walls power units id with
                    | true, UnitDied(Elf, _) -> None
                    | _, NothingHappend units
                    | _, UnitDied (_, units) -> turns units rest

        turns units order
    round 0 units

let calculateOutcome (rounds, units) = rounds * (Map.values units |> Seq.sumBy _.Hp)

let part1 input =
    match combat false 3 (parse input) with
    | Some outcome -> calculateOutcome outcome
    | None -> failwith "Bad input?"

part1 input

// %%

let part2 input =
    let walls, units = parse input
    Seq.initInfinite ((+) 4)
    |> Seq.pick (fun power -> combat true power (walls, units))
    |> calculateOutcome

part2 input

// %%

#load "../utils.fsx"
open System
open System.Runtime.Serialization
open Utils

let input = GetData 24
let example = """Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"""
example

// %%
type Modifier = Immune of string | WeakTo of string
type Army = ImmuneSystem | Infection

type Group = { Army: Army
               Id: int
               Units: int
               Hp: int
               Modifiers: Modifier array
               Damage: int
               DamageType: string
               Initiative: int }

let parseWeaknessGroup (grp:string) =
    match grp with
    | Regex "immune to (.+)" [str] -> Split "," str |> Array.map Immune
    | Regex "weak to (.+)" [str] -> Split "," str |> Array.map WeakTo

let parseModifier str =
    Split ";" str
    |> Array.collect parseWeaknessGroup

let parseGroup army input=
    let matches = input |> Matches "(\d+) units each with (\d+) hit points (?:\((.+)\))?\s?with an attack that does (\d+) (.+) damage at initiative (\d+)"
    match matches with
    | [Int units; Int hp; modifier; Int damage; damageType; Int initiative] ->
        { Army = army; Id = 0; Units = units; Hp = hp; Modifiers = parseModifier modifier; Damage = damage; DamageType = damageType; Initiative = initiative }
    | [Int units; Int hp; Int damage; damageType; Int initiative] ->
        { Army = army; Id = 0; Units = units; Hp = hp; Modifiers = [||]; Damage = damage; DamageType = damageType; Initiative = initiative }
    | v -> failwith $"{v} - {input}"

let parseArmy input =
    let lines = GetLines input
    let army = if lines[0] = "Immune System:" then ImmuneSystem else Infection
    lines[1..] |> Array.map (parseGroup army)

let parse input =
    GetBlocks input
    |> Array.collect parseArmy
    |> Array.mapi (fun i g -> {g with Id = i})
    
parse example

// %%
// initiative as tiebreaker
let effectivePower group = group.Units * group.Damage
let effectivePowerWithTiebreaker group = effectivePower group, group.Initiative

let getModifier attacker defender =
    if Array.contains (Immune attacker.DamageType) defender.Modifiers then 0
    elif Array.contains (WeakTo attacker.DamageType) defender.Modifiers then 2
    else 1

let getDamage attacker defender =
    let effectivePower = effectivePower attacker
    let modifier = getModifier attacker defender
    effectivePower * modifier

let selectTarget (targets: (int*int) list,remainingGroups) attacker =
    let target =
        remainingGroups
        |> Seq.filter (fun x -> x.Army <> attacker.Army)
        |> Seq.filter (fun d -> getDamage attacker d > 0)
        |> Seq.sortByDescending (fun d -> getDamage attacker d, effectivePower d, d.Initiative)
        |> Seq.tryHead
    match target with
    | None -> targets, remainingGroups
    | Some target -> (attacker.Id, target.Id)::targets, Array.filter (fun x -> x.Id <> target.Id) remainingGroups 

let targetSelection groups =
    let targetSelectionOrder = groups |> Array.sortByDescending effectivePowerWithTiebreaker
    let (targets, _) = Array.fold (selectTarget) ([],groups) targetSelectionOrder
    targets |> Map.ofList

parse example |> targetSelection

// %%
let attackTarget attacker defender =
    getDamage attacker defender

let findTarget groupMap targets attackerId =
    Map.tryFind attackerId targets
    |> Option.bind (fun id -> Map.tryFind id groupMap)
    
let performAttack attacker defender =
    let damage = getDamage attacker defender
    let unitsLost = damage / defender.Hp
    {defender with Units = max 0 (defender.Units - unitsLost)}

let rec attack targets (groupMap: Map<int,Group>) attackerId =
    let attacker = Map.tryFind attackerId groupMap
    let target = findTarget groupMap targets attackerId
    match attacker, target with
    | Some attacker, Some target when attacker.Units > 0 ->
        let defender = performAttack attacker target
        if defender.Units > 0 then Map.add defender.Id defender groupMap
        else Map.remove defender.Id groupMap
    | _ -> groupMap

let attackPhase groups targets =
    let initiativeOrder = groups |> Array.sortByDescending _.Initiative |> Array.map _.Id
    let groupMap = Array.map (fun g -> g.Id, g) groups |> Map.ofArray
    Array.fold (attack targets) groupMap initiativeOrder
    |> Map.values
    |> Seq.toArray
    
// %%
type Outcome = Victory of Army * int | Stalemate | NextFight of Group array

let getUnitsPerArmy groups =
    groups
    |> Array.groupBy _.Army
    |> Array.map (fun (a,items) -> a, items |> Array.sumBy _.Units )
    |> Array.sortBy fst

let rec fight groups =
    let before = getUnitsPerArmy groups
    let groups = groups |> targetSelection |> attackPhase groups
    let after = getUnitsPerArmy groups
    if before = after then Stalemate
    else
        match after with
        | [| result |] -> Victory result
        | _ -> fight groups

let part1 input =
    let groups = parse input
    fight groups

part1 example

// %%
part1 input

// %%
let boostGroup boost group =
    if group.Army = ImmuneSystem then
        {group with Damage = group.Damage + boost}
    else group
let boostImmuneSystem groups boost =
    groups |> Array.map (boostGroup boost )

let tryFightWithBoost groups boost =
    let groups = boostImmuneSystem groups boost
    match fight groups with
    | Victory (ImmuneSystem, result) -> Some result
    | _ -> None

let part2 input =
    let groups = parse input
    
    Seq.initInfinite id
    |> Seq.pick (tryFightWithBoost groups)
    
part2 example
    

// %%
part2 input
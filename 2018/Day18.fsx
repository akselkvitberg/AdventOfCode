#load "../utils.fsx"
open Utils

let input = GetData 18
let example = """.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|."""
example

// %%
type Acre = Trees | Lumberyard | Open

let parse input =
    ToGrid input
    |> Map.ofArray
    |> Map.map (fun _ v -> match v with '|' -> Trees | '#' -> Lumberyard | _ -> Open)

// %%
let getNeighbours (x,y) =
    [
        (x-1, y-1)
        (x, y-1)
        (x+1,y-1)
        (x+1, y)
        (x-1, y)
        (x-1, y+1)
        (x, y+1)
        (x+1,y+1)
    ]

let rules grid pos acre =
    let neighbours = getNeighbours pos |> List.choose (fun p -> Map.tryFind p grid)
    let trees = neighbours |> List.filter ((=) Trees) |> List.length
    let lumberyards = neighbours |> List.filter ((=) Lumberyard) |> List.length
    
    match acre with
    | Open when trees >= 3 -> Trees
    | Trees when lumberyards >= 3 -> Lumberyard
    | Lumberyard when lumberyards >= 1 && trees >= 1 -> Lumberyard
    | Lumberyard -> Open
    | _ -> acre

let oneMinute grid = Map.map (rules grid) grid

//parse example |> oneMinute
// %%
let calculate minutes grid =
    List.fold (fun s _ -> oneMinute s) grid [1..minutes]
    |> Map.toList
    |> List.countBy snd
    |> Map.ofList
    |> fun m -> (Map.tryFind Lumberyard m |> Option.defaultValue 0) * (Map.tryFind Trees m |> Option.defaultValue 0)

let part1 input =
    parse input
    |> calculate 10
part1 example

// %%
part1 input

// %%
let part2 input =
    let grid = parse input
    Seq.unfold (fun grid -> Some (grid, oneMinute grid)) grid
    |> Seq.indexed
    |> Seq.scan (fun (seen, _) (minute, g) ->
        match Map.tryFind g seen with
        | Some first -> seen, Some(first,minute)
        | None -> Map.add g minute seen, None) (Map.empty, None)
    |> Seq.pick snd
    |> fun (first, repeat) -> first + (1_000_000_000 - first) % (repeat - first)
    |> fun minutes -> calculate minutes grid
part2 example

// %%
part2 input

// %%
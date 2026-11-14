#load "../utils.fsx"
open Utils

let input = GetRawData 13
let example1 = """/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   """


// %%
type Vec2 = int*int
let up = 0, -1
let down = 0, 1
let left = -1, 0
let right = 1, 0

type Item = Intersection | Slash | Backslash
type Cart = Vec2 * Vec2 * int
let parse input =
    let grid = ToGrid input
    let carts =
        grid
        |> Array.choose (fun (p,v) ->
            match v with
            | '^' -> Some (p, up, 0)
            | 'v' -> Some (p, down, 0)
            | '<' -> Some (p, left, 0)
            | '>' -> Some (p, right, 0)    
            | _ -> None)
        |> Array.toList
    let tracks =
        grid
        |> Array.choose (fun (p,v) ->
            match v with
            | '+' -> Some (p, Intersection)
            | '/' -> Some (p, Slash)
            | '\\' -> Some (p, Backslash)
            | _ -> None)
        |> Map.ofArray
    
    carts, tracks
parse example1

// %%
let turnRight (dx, dy) = (-dy, dx)
let turnLeft  (dx, dy) = (dy, -dx)
let flipRight (dx, dy) = (-dy, -dx)
let flipLeft (dx, dy) = (dy, dx)
let move (x,y) (dx,dy) = (x + dx, y+dy)

 
let moveCart tracks (pos, dir, nextDirection) =
    let pos = move pos dir
    let (dir, nextDirection) =
        match Map.tryFind pos tracks with
        | Some Backslash -> flipLeft dir, nextDirection
        | Some Slash -> flipRight dir, nextDirection
        | Some Intersection when nextDirection = 0 -> turnLeft dir, nextDirection + 1
        | Some Intersection when nextDirection = 1 -> dir, nextDirection + 1
        | Some Intersection when nextDirection = 2 -> turnRight dir, 0
        | _ -> dir, nextDirection

    pos, dir, nextDirection

// %%
let tick tracks carts =
    let sortedCarts = List.sortBy (fun ((x,y), _, _) -> y,x) carts
    
    let rec tickCart moved remaining =
        match remaining with
        | [] -> Ok moved
        | cart :: rest ->
            let (pos, _, _) as cart = moveCart tracks cart
            let hits (p, _, _) = p = pos
            if List.exists hits moved || List.exists hits rest then Error pos
            else tickCart (cart::moved) rest

    tickCart [] sortedCarts

let part1 input =
    let carts, tracks = parse input
    let rec run carts =
        match tick tracks carts with
        | Error pos -> pos
        | Ok carts -> run carts
    run carts
    |> fun (x,y) -> $"{x},{y}"

part1 example1

// %%
part1 input

// %%
let example2 = """/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/"""

// %%

let tickWithRemoval tracks carts =
    let sortedCarts = List.sortBy (fun ((x,y), _, _) -> y,x) carts
    
    let rec tickCart moved remaining =
        match remaining with
        | [] -> moved
        | cart :: rest ->
            let (pos, _, _) as cart = moveCart tracks cart
            let hits (p, _, _) = p = pos
            if List.exists hits moved || List.exists hits rest
                then tickCart (List.filter (hits >> not) moved) (List.filter (hits >> not) rest)
                else tickCart (cart::moved) rest

    tickCart [] sortedCarts

let part2 input =
    let carts, tracks = parse input
    let rec run carts =
        match tickWithRemoval tracks carts with
        [ remaining ] -> remaining
        | carts -> run carts
    run carts
    |> fun ((x,y), _, _) -> $"{x},{y}"

part2 example2

// %%
part2 input
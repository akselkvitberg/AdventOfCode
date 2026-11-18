#load "../utils.fsx"
open Utils

let input = GetData 17
let example = GetExample 17 1
example

// %%

type Rect = { X1: int; X2: int; Y1: int; Y2:int }
let parseLine line =
    match line with
    | Regex "x=(\d+), y=(\d+)\.\.(\d+)" [Int x; Int y1; Int y2] -> {X1 = x;  X2 = x;  Y1 = y1; Y2 = y2}
    | Regex "y=(\d+), x=(\d+)\.\.(\d+)" [Int y; Int x1; Int x2] -> {X1 = x1; X2 = x2; Y1 = y; Y2 =  y}

type Tile = Sand | Clay | StillWater | FlowingWater

let parse input =
    let rects = GetLines input |> Array.map parseLine
    let maxX, maxY = rects |> Array.map _.X2 |> Array.max, rects |> Array.map _.Y2 |> Array.max
    let minX, minY = rects |> Array.map _.X1 |> Array.min, rects |> Array.map _.Y1 |> Array.min
    let width = maxX + 2
    let grid = Array2D.create (maxY+1) width Sand

    for r in rects do
        for y in r.Y1..r.Y2 do
        for x in r.X1..r.X2 do
            grid[y,x] <- Clay
    
    grid, minY, maxY, width
    
//parse example

// %%
/// Prints the grid, trimmed to the columns in use plus one on each side
let drawGrid (grid: Tile[,]) =
    let height, width = Array2D.length1 grid, Array2D.length2 grid
    let usedColumns =
        [ for x in 0 .. width - 1 do
              if Seq.exists (fun y -> grid[y, x] <> Sand) [ 0 .. height - 1 ] then x ]
    let fromX = max 0 (List.min usedColumns - 1)
    let toX = min (width - 1) (List.max usedColumns + 1)

    for y in 0 .. height - 1 do
        [| for x in fromX .. toX ->
               if (y, x) = (0, 500) then '+'
               else
                   match grid[y, x] with
                   | Sand -> '.'
                   | Clay -> '#'
                   | StillWater -> '~' 
                   | FlowingWater -> '|'|]
        |> System.String
        |> printfn "%s"
    printfn ""

//parse example |> fun (grid, _, _) -> drawGrid grid |> drawGrid |> ignore

// %%
let flow (grid: Tile[,]) maxY =
    let isFloor item = item = Clay || item = StillWater

    let rec verticalFlow x y =
        if y <= maxY && grid[y, x] = Sand then
            grid[y, x] <- FlowingWater
            if y < maxY then
                if grid[y + 1, x] = Sand then verticalFlow x (y + 1)
                if isFloor grid[y + 1, x] then fill x y

    and fill x y =
        let leftX, leftWall = horizontalFlow x y -1
        let rightX, rightWall = horizontalFlow x y 1
        if leftWall && rightWall then
            for x in leftX .. rightX do
                grid[y, x] <- StillWater

    and horizontalFlow x y dx =
        if grid[y, x + dx] = Clay then x, true
        else
            let x = x + dx
            grid[y, x] <- FlowingWater
            if grid[y + 1, x] = Sand then verticalFlow x (y + 1)
            if isFloor grid[y + 1, x] then horizontalFlow x y dx else x, false

    verticalFlow 500 0
    grid
        
parse example |> fun (grid, _, maxY, _) -> flow grid maxY |> drawGrid
// %%

let settleWaterAndCalculateWater input filter  =
    let grid, minY, maxY, width = parse input
    let grid = flow grid maxY
    grid[minY..maxY, *]
    |> Seq.cast<Tile>
    |> Seq.filter filter
    |> Seq.length
    
let part1 input =
    settleWaterAndCalculateWater input (fun x -> x = FlowingWater || x = StillWater)
part1 example
// %%
part1 input

// %%
let part2 input =
    settleWaterAndCalculateWater input ((=) StillWater)

part2 example

// %%
part2 input
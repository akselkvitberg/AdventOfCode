#load "../utils.fsx"
open Utils

let input = GetData 14
input

// %%

let part1 input =
    let targetRecipes = int input
    let recipeScores = ResizeArray<int>(targetRecipes+12)
    recipeScores.Add 3
    recipeScores.Add 7

    let rec combineRecipes a b =
        if recipeScores.Count > targetRecipes+10 then (a,b)
        else
            let combinedScore = recipeScores[a] + recipeScores[b]
                
            if combinedScore < 10 then
                recipeScores.Add combinedScore
            else
                let s1 = (combinedScore/10) % 10
                let s2 = combinedScore % 10
                recipeScores.Add s1
                recipeScores.Add s2
            
            let a = (a + recipeScores[a] + 1) % recipeScores.Count
            let b = (b + recipeScores[b] + 1) % recipeScores.Count
            combineRecipes a b
            
    let _ = combineRecipes 0 1
    recipeScores.Slice(targetRecipes, 10) |> Seq.map string |> String.concat ""

part1 "9"

// %%
part1 input

// %%
let part2 input =
    let targetScores = input |> Seq.map (string >> int) |> Seq.toArray
    let length = targetScores.Length
    let recipeScores = ResizeArray<int>()
    recipeScores.Add 3
    recipeScores.Add 7
    
    let isMatch offset =
        let start = recipeScores.Count - length - offset
        if start < 0 then None else
        if start >= 0 && Seq.forall (fun i -> recipeScores[start + i] = targetScores[i]) (seq { 0 .. length - 1 })
        then Some start else None

    let rec combineRecipes a b =
        match (isMatch 0, isMatch 1) with
        | _, Some ix | Some ix, _ -> ix
        | _ -> 
            let combinedScore = recipeScores[a] + recipeScores[b]
            if combinedScore < 10 then
                recipeScores.Add combinedScore
            else
                let s1 = (combinedScore/10) % 10
                let s2 = combinedScore % 10
                recipeScores.Add s1
                recipeScores.Add s2
            
            let a = (a + recipeScores[a] + 1) % recipeScores.Count
            let b = (b + recipeScores[b] + 1) % recipeScores.Count
            combineRecipes a b
    
    combineRecipes 0 1
    
part2 "51589"

// %%
part2 input

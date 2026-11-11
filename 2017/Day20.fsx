#load "../utils.fsx"
open Utils

let input = GetData 20
let example = """p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>
p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>

p=<4,0,0>, v=<1,0,0>, a=<-1,0,0>
p=<2,0,0>, v=<-2,0,0>, a=<-2,0,0>

p=<4,0,0>, v=<0,0,0>, a=<-1,0,0>
p=<-2,0,0>, v=<-4,0,0>, a=<-2,0,0>

p=<3,0,0>, v=<-1,0,0>, a=<-1,0,0>
p=<-8,0,0>, v=<-6,0,0>, a=<-2,0,0>"""
example

// %%
type Vec3 = { X: int64; Y: int64; Z: int64 }
module Vec3 =
    let Add (a:Vec3) (b:Vec3) = {X = a.X + b.X; Y = a.Y + b.Y; Z = a.Z + b.Z}
    let Abs (a: Vec3) = abs a.X + abs a.Y + abs a.Z
    
type Particle = {
    Id: int
    Position: Vec3
    Velocity: Vec3
    Acceleration: Vec3
}
let parseParticle id line =
    match line with
    | Regex "p=\<(.+),(.+),(.+)\>, v=\<(.+),(.+),(.+)\>, a=\<(.+),(.+),(.+)\>" [Long px; Long py; Long pz; Long vx; Long vy; Long vz; Long ax; Long ay; Long az] ->
        {
            Id = id
            Position = {X = px; Y = py; Z = pz}
            Velocity = {X = vx; Y = vy; Z = vz}
            Acceleration = {X = ax; Y = ay; Z = az}
        }
    | line -> failwith line

let parse input =
    GetLines input
    |> Array.mapi parseParticle

parse example

// %%

let part1 input =
    let particles = parse input
    // "in the long term" - acceleration always determines this, in the long term
    // so sort by acc?
    particles |> Array.minBy (fun p -> Vec3.Abs p.Acceleration, Vec3.Abs p.Velocity, Vec3.Abs p.Position) |> _.Id

part1 example

// %%
part1 input

// %%
let tickParticle p =
    let velocity = Vec3.Add p.Velocity p.Acceleration
    let position = Vec3.Add p.Position velocity
    { p with Velocity = velocity; Position = position }

let removeCollisions particles =
    particles
    |> Array.groupBy _.Position
    |> Array.filter (fun (_, group) -> group.Length = 1)
    |> Array.collect snd
    
let timeStep particles =
    particles
    |> Array.map tickParticle
    |> removeCollisions

let part2 input =
    let particles = parse input
    [1..1000]
    |> List.fold (fun p _ -> timeStep p) particles
    |> Array.length

let example2 = """p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
p=< 3,0,0>, v=<-1,0,0>D, a=< 0,0,0>"""

part2 example2 // 1

// %%
part2 input

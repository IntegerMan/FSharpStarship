namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects

module WorldBuilding =

  let private makeHorizontalArea tileType width startPos = [ for i in 0 .. width - 1 -> startPos |> offset i 0 |> makeTile tileType]
  let private makeVerticalArea tileType height startPos = [ for i in 0 .. height - 1 -> startPos |> offset 0 i |> makeTile tileType]

  let private makeHorizontalWall = makeHorizontalArea Wall
  let private makeVerticalWall = makeVerticalArea Wall

  let private makeArea tileType width height startPos =
    [for y in 0 .. height - 1 do
      for x in 0 .. width - 1 do
        yield makeTile tileType (startPos |> offset x y)
    ]

  let private makeFloor = makeArea Floor

  let private makeRoom startPos width height =
    (startPos |> makeHorizontalWall width) @
    (startPos |> offset 0 (height - 1) |> makeHorizontalWall width) @
    (startPos |> offset 0 1 |> makeVerticalWall (height - 2)) @
    (startPos |> offset (width-1) 1 |> makeVerticalWall (height - 2)) @
    (startPos |> offset 1 1 |> makeArea Floor (width - 2) (height - 2))

  let private replaceListItem (newItem: Tile) (list: List<Tile>): List<Tile> = list |> List.map(fun i -> if i.Pos = newItem.Pos then newItem else i)

  let private getTiles() = 
    let space = pos 0 0 |> makeArea Space 28 17

    // Outer Structural Walls
    (pos 1 1 |> makeHorizontalWall 20)
    @ (pos 1 15 |> makeHorizontalWall 20)
    // Upper Engine Pod
    @ (pos 1 5 |> makeHorizontalWall 6)
    @ (pos 1 2 |> makeVerticalWall 3)
    @ (pos 2 2 |> makeFloor 8 3)
    // Lower Engine Pod
    @ (pos 1 11 |> makeHorizontalWall 6)
    @ (pos 1 12 |> makeVerticalWall 3)
    @ (pos 2 12 |> makeFloor 8 3)
    // Engineering
    @ (pos 6 6 |> makeVerticalWall 5)
    @ (pos 7 5 |> makeFloor 3 7)
    @ [pos 10 8 |> makeTile Floor]
    // Main Hallway
    @ (pos 11 7 |> makeFloor 10 3)
    @ [pos 11 6 |> makeTile Wall]
    @ [pos 11 10 |> makeTile Wall]
    @ (pos 13 6 |> makeHorizontalWall 4)
    @ (pos 18 6 |> makeHorizontalWall 2)
    @ (pos 13 10 |> makeHorizontalWall 4)
    @ (pos 18 10 |> makeHorizontalWall 2)
    // Upper Room 1
    @ (pos 10 2 |> makeVerticalWall 6)
    @ (pos 11 2 |> makeFloor 3 4)
    @ [pos 12 6 |> makeTile Floor]
    // Lower Room 1
    @ (pos 10 9 |> makeVerticalWall 6)
    @ (pos 11 11 |> makeFloor 3 4)
    @ [pos 12 10 |> makeTile Floor]
    // Upper Room 2
    @ (pos 14 2 |> makeVerticalWall 4)
    @ (pos 20 2 |> makeVerticalWall 6)
    @ (pos 15 2 |> makeFloor 5 4)
    @ [pos 17 6 |> makeTile Floor]
    // Lower Room 2
    @ (pos 14 11 |> makeVerticalWall 4)
    @ (pos 20 10 |> makeVerticalWall 5)
    @ (pos 15 11 |> makeFloor 5 4)
    @ [pos 17 10 |> makeTile Floor]
    // Cockpit
    @ [pos 20 8 |> makeTile Floor]
    @ (pos 21 3 |> makeHorizontalWall 4)
    @ (pos 21 13 |> makeHorizontalWall 4)
    @ (pos 24 4 |> makeVerticalWall 3)
    @ (pos 24 10 |> makeVerticalWall 3)
    @ (pos 21 4 |> makeFloor 3 9)
    @ (pos 24 7 |> makeFloor 2 3)
    @ (pos 25 6 |> makeHorizontalWall 2)
    @ (pos 25 10 |> makeHorizontalWall 2)
    @ (pos 26 7 |> makeVerticalWall 3)
    @ [pos 20 9 |> makeTile Wall]
    |> List.fold(fun space t -> space |> replaceListItem t) space

  let private getObjects() = [
      {Pos=pos 17 3; ObjectType=Astronaut}
      {Pos=pos 20 8; ObjectType=Door(IsOpen=false, IsHorizontal=true)}
      {Pos=pos 10 8; ObjectType=Door(IsOpen=false, IsHorizontal=true)}
      {Pos=pos 12 6; ObjectType=Door(IsOpen=false, IsHorizontal=false)}
      {Pos=pos 12 10; ObjectType=Door(IsOpen=false, IsHorizontal=false)}
      {Pos=pos 17 10; ObjectType=Door(IsOpen=false, IsHorizontal=false)}
      {Pos=pos 17 6; ObjectType=Door(IsOpen=false, IsHorizontal=false)}
      {Pos=pos 18 11; ObjectType=Bed(IsLeft=true)}
      {Pos=pos 19 11; ObjectType=Bed(IsLeft=false)}
    ]

  let generateWorld(): GameWorld = { Tiles=getTiles(); Objects=getObjects() }


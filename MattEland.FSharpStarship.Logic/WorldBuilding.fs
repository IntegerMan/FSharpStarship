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
  let private makeDoor = makeTile (Door(IsOpen=false))

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
    (pos 1 2 |> makeHorizontalWall 20)
    @ (pos 1 16 |> makeHorizontalWall 20)
    // Upper Engine Pod
    @ (pos 1 6 |> makeHorizontalWall 6)
    @ (pos 1 3 |> makeVerticalWall 3)
    @ (pos 2 3 |> makeFloor 8 3)
    // Lower Engine Pod
    @ (pos 1 12 |> makeHorizontalWall 6)
    @ (pos 1 13 |> makeVerticalWall 3)
    @ (pos 2 13 |> makeFloor 8 3)
    // Engineering
    @ (pos 6 7 |> makeVerticalWall 5)
    @ (pos 7 6 |> makeFloor 3 7)
    @ [pos 10 9 |> makeDoor]
    // Main Hallway
    @ (pos 11 8 |> makeFloor 10 3)
    @ [pos 11 7 |> makeTile Wall]
    @ [pos 11 11 |> makeTile Wall]
    @ (pos 13 7 |> makeHorizontalWall 4)
    @ (pos 18 7 |> makeHorizontalWall 2)
    @ (pos 13 11 |> makeHorizontalWall 4)
    @ (pos 18 11 |> makeHorizontalWall 2)
    // Upper Room 1
    @ (pos 10 3 |> makeVerticalWall 6)
    @ (pos 11 3 |> makeFloor 3 4)
    @ [pos 12 7 |> makeDoor]
    // Lower Room 1
    @ (pos 10 10 |> makeVerticalWall 6)
    @ (pos 11 12 |> makeFloor 3 4)
    @ [pos 12 11 |> makeDoor]
    // Upper Room 2
    @ (pos 14 3 |> makeVerticalWall 4)
    @ (pos 20 3 |> makeVerticalWall 6)
    @ (pos 15 3 |> makeFloor 5 4)
    @ [pos 17 7 |> makeDoor]
    // Lower Room 2
    @ (pos 14 12 |> makeVerticalWall 4)
    @ (pos 20 11 |> makeVerticalWall 5)
    @ (pos 15 12 |> makeFloor 5 4)
    @ [pos 17 11 |> makeDoor]
    // Cockpit
    @ [pos 20 9 |> makeDoor]
    @ (pos 21 4 |> makeHorizontalWall 4)
    @ (pos 21 14 |> makeHorizontalWall 4)
    @ (pos 24 5 |> makeVerticalWall 3)
    @ (pos 24 11 |> makeVerticalWall 3)
    @ (pos 21 5 |> makeFloor 3 9)
    @ (pos 24 8 |> makeFloor 2 3)
    @ (pos 25 7 |> makeHorizontalWall 2)
    @ (pos 25 11 |> makeHorizontalWall 2)
    @ (pos 26 8 |> makeVerticalWall 3)
    @ [pos 20 10 |> makeTile Wall]
    |> List.fold(fun space t -> space |> replaceListItem t) space

  let private getObjects() = [
      {Pos={X=17; Y=4}; ObjectType=Astronaut}
      //{Pos={X=8; Y=3}; ObjectType=AirScrubber}
    ]

  let generateWorld(): GameWorld = { Tiles=getTiles(); Objects=getObjects() }


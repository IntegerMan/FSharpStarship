namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects

module WorldBuilding =

  // TODO: It'd be better to have makeX below this take in a function that just takes a position
  let private makeHorizontalArea (startPos, tileType, width) = [ for i in 0 .. width - 1 -> makeTile(tileType, startPos |> offset i 0)]
  let private makeVerticalArea (startPos, tileType, height) = [ for i in 0 .. height - 1 -> makeTile(tileType, startPos |> offset 0 i)]

  let private makeArea (startPos, tileType, width, height): List<Tile> =     
    [for y in 0 .. height - 1 do
      for x in 0 .. width - 1 do
        yield makeTile(tileType, startPos |> offset x y)]

  let private makeRoom (startPos, width, height): List<Tile> =
    makeHorizontalArea(startPos, Wall, width) @
    makeHorizontalArea(startPos |> offset 0 (height - 1), Wall, width) @
    makeVerticalArea(startPos |> offset 0 1, WallLeft, height - 2) @
    makeVerticalArea(startPos |> offset (width - 1) 1, WallRight, height - 2) @
    makeArea(startPos |> offset 1 1, Floor, width - 2, height - 2)

  let private replaceListItem (newItem: Tile) (list: List<Tile>): List<Tile> = list |> List.map(fun i -> if i.Pos = newItem.Pos then newItem else i)

  let private getTiles() = 
    let space = [for y in 0 .. 10 do
                  for x in 0 .. 14 do
                    yield makeTile(TileType.Space, {X=x;Y=y})
                ]

    makeRoom({X=1; Y=1}, 13, 9) 
    |> replaceListItem (makeTile(TileType.Floor, {X=5; Y=1}))
    |> List.fold(fun space t -> space |> replaceListItem t) space

  let private getObjects() = [
      {Pos={X=6; Y=3}; ObjectType=Astronaut}
      {Pos={X=8; Y=3}; ObjectType=AirScrubber}
    ]

  let generateWorld(): GameWorld = { Tiles=getTiles(); Objects=getObjects() }


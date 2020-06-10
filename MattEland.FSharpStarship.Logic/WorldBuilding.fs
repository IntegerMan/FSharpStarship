namespace MattEland.FSharpStarship.Logic

open Positions
open World

module WorldBuilding =

  // TODO: It'd be better to have makeX below this take in a function that just takes a position
  let private makeHorizontalArea (startPos, tileType, width) = [ for i in 0 .. width - 1 -> makeTile(tileType, offset(startPos, i, 0))]
  let private makeVerticalArea (startPos, tileType, height) = [ for i in 0 .. height - 1 -> makeTile(tileType, offset(startPos, 0, i))]

  let private makeArea (startPos, tileType, width, height): List<Tile> =     
    [for y in 0 .. height - 1 do
      for x in 0 .. width - 1 do
        yield makeTile(tileType, offset(startPos, x, y))]

  let private makeRoom (startPos, width, height): List<Tile> =
    makeHorizontalArea(offset(startPos, 0, 0), Wall, width) @
    makeHorizontalArea(offset(startPos, 0, height - 1), Wall, width) @
    makeVerticalArea(offset(startPos, 0, 1), WallLeft, height - 2) @
    makeVerticalArea(offset(startPos, width - 1, 1), WallRight, height - 2) @
    makeArea(offset(startPos, 1, 1), Floor, width - 2, height - 2)

  let private replaceListItem pos newItem list = 
    list |> List.map(fun i -> if i.pos = pos then newItem else i)

  let private getTiles(): list<Tile> = 
    let voidPos = {x=3; y=1}
    let voidTile = makeTile(TileType.Floor, voidPos)

    let mutable space =
      [for y in 0 .. 10 do
        for x in 0 .. 14 do
          yield makeTile(TileType.Space, {x=x;y=y})]

    let placedTiles = makeRoom({x=1; y=1}, 13, 9) |> replaceListItem voidPos voidTile

    placedTiles |> List.iter(fun t -> space <- replaceListItem t.pos t space)

    space

  let private getObjects(): list<GameObject> = [{pos={x=3; y=3}; objectType=Player}]

  let generateWorld(): GameWorld = { tiles=getTiles(); objects=getObjects() }


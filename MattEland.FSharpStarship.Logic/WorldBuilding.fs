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
    makeHorizontalArea(startPos, Wall, width) @
    makeHorizontalArea(offset(startPos, 0, height - 1), Wall, width) @
    makeVerticalArea(offset(startPos, 0, 1), Wall, height - 2) @
    makeVerticalArea(offset(startPos, width - 1, 1), Wall, height - 2) @
    makeArea(offset(startPos, 1, 1), Floor, width - 2, height - 2)

  let private getTiles(): list<Tile> = makeRoom({x=3; y=4}, 13, 9)

  let generateWorld(): GameWorld = { tiles=getTiles(); objects=[] }


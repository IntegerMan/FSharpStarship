namespace MattEland.FSharpStarship.Logic

open Positions
open World

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

  let private replaceListItem pos newItem list = 
    list |> List.map(fun i -> if i.pos = pos then newItem else i)

  let private getTiles(): list<Tile> = 
    let space = [for y in 0 .. 10 do
                  for x in 0 .. 14 do
                    yield makeTile(TileType.Space, {X=x;Y=y})
                ]

    makeRoom({X=1; Y=1}, 13, 9) 
    |>  List.fold(fun space t -> space |> replaceListItem t.pos t) space

  let private getObjects(): list<GameObject> = [
      {pos={X=3; Y=3}; objectType=Astronaut}
      {pos={X=7; Y=5}; objectType=Astronaut}
      {pos={X=8; Y=3}; objectType=AirScrubber}
    ]

  let generateWorld(): GameWorld = { tiles=getTiles(); objects=getObjects() }


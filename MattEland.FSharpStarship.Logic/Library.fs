namespace MattEland.FSharpStarship.Logic

module Common =

  type TileType =
    | Space
    | Floor
    | Wall
    | Pipe
    | Pump
    | Tank
    | Cable
    | Door
    | Capacitor
    | PowerPlant
    | HeatSink
    | Vent
    | Crate
    | Cabinet
    | Workstation
    | Terminal
    | Table
    | Chair
    | Toilet
    | Hydroponics
    | FoodPrep

  type Pos = {x: int; y: int}

  let offset(pos: Pos, xDelta, yDelta): Pos = {x=pos.x + xDelta; y = pos.y + yDelta}

  type Tile = {tileType: TileType; pos: Pos}

  type RGB = {r: byte; g: byte; b: byte}

  let getBackgroundColor (tile: Tile): RGB =
    match tile.tileType with
    | Floor -> {r=86uy; g=86uy; b=128uy}
    | Wall -> {r=64uy; g=64uy; b=84uy}
    | _ -> {r=255uy; g=0uy; b=255uy}

  let makeFloor pos = {tileType=Floor; pos=pos}
  let makeWall pos = {tileType=Wall; pos=pos}

  let makeHorizontalWall (startPos, width) = [ for i in 0 .. width - 1 -> makeWall(offset(startPos, i, 0))]
  let makeVerticalWall (startPos, height) = [ for i in 0 .. height - 1 -> makeWall(offset(startPos, 0, i))]

  let makeArea (startPos, tileType, width, height): List<Tile> =     
    [for y in 0 .. height - 1 do
      for x in 0 .. width - 1 do
        yield {tileType=tileType; pos=offset(startPos, x, y)}]


  let getTiles(): list<Tile> = 
    makeHorizontalWall({x=4; y=1}, 7) @
    makeHorizontalWall({x=4; y=7}, 7) @
    makeVerticalWall({x=4; y=2}, 5) @
    makeVerticalWall({x=10; y=2}, 5) @
    makeArea({x=5;y=2}, Floor, 5, 5)

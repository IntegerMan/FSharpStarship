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

  let defaultHeat: int = 0;

  type Tile = {tileType: TileType; pos: Pos; heat: int; oxygen: decimal; carbonDioxide: decimal}

  type RGB = {r: byte; g: byte; b: byte}

  let getBackgroundColor (tile: Tile): RGB =
    match tile.tileType with
    | Floor -> {r=86uy; g=86uy; b=128uy}
    | Wall -> {r=64uy; g=64uy; b=84uy}
    | _ -> {r=255uy; g=0uy; b=255uy}

  
  let makeTile(tileType, pos) = 
    {
      tileType=tileType; 
      pos=pos; 
      heat=defaultHeat; 
      oxygen=0.7M; 
      carbonDioxide=0.3M
    } 
    
  // TODO: It'd be better to have makeX below this take in a function that just takes a position
  let makeHorizontalArea (startPos, tileType, width) = [ for i in 0 .. width - 1 -> makeTile(tileType, offset(startPos, i, 0))]
  let makeVerticalArea (startPos, tileType, height) = [ for i in 0 .. height - 1 -> makeTile(tileType, offset(startPos, 0, i))]

  let makeArea (startPos, tileType, width, height): List<Tile> =     
    [for y in 0 .. height - 1 do
      for x in 0 .. width - 1 do
        yield makeTile(tileType, offset(startPos, x, y))]

  let makeRoom (startPos, width, height): List<Tile> =
    makeHorizontalArea(startPos, Wall, width) @
    makeHorizontalArea(offset(startPos, 0, height - 1), Wall, width) @
    makeVerticalArea(offset(startPos, 0, 1), Wall, height - 2) @
    makeVerticalArea(offset(startPos, width - 1, 1), Wall, height - 2) @
    makeArea(offset(startPos, 1, 1), Floor, width - 2, height - 2)

  let getTiles(): list<Tile> = 
    makeRoom({x=3; y=4}, 13, 9)

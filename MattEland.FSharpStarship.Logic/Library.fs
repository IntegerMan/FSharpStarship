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

  type Tile = {tileType: TileType; pos: Pos}

  type RGB = {r: byte; g: byte; b: byte}

  let getBackgroundColor (tile: Tile): RGB =
    match tile.tileType with
    | Floor -> {r=86uy; g=86uy; b=128uy}
    | Wall -> {r=64uy; g=64uy; b=84uy}
    | _ -> {r=255uy; g=0uy; b=255uy}

  let makeWall pos = {tileType=Wall; pos=pos}
  let makeFloor pos = {tileType=Floor; pos=pos}

  let getTiles(): list<Tile> = 
    [
      makeWall({x=5; y=1});
      makeWall({x=6; y=1});
      makeFloor({x=5; y=2});
      makeFloor({x=6; y=2});
    ]

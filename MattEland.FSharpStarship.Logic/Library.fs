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

  let makeWall pos = {tileType=Wall; pos=pos}
  let makeFloor pos = {tileType=Floor; pos=pos}

  let getTiles(): list<Tile> = 
    [
      makeWall({x=5; y=1});
      makeWall({x=6; y=1});
      makeFloor({x=5; y=2});
      makeFloor({x=6; y=2});
    ]

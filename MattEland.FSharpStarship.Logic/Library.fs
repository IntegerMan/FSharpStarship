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

  type Pos(x, y) = 
    let x: int = x;
    let y: int = y;

  type Tile(tileType, pos) =
    let tileType: TileType = tileType
    let pos: Pos = pos

  let getTiles: list<Tile> =
    [new Tile(Floor, new Pos(5, 2))]

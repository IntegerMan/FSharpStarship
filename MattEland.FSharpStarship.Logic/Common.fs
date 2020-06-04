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

  let private defaultHeat: decimal = 0M;

  type Tile = 
    {
      tileType: TileType; 
      pos: Pos; 
      heat: decimal; 
      // TODO: Probably should have a gas composition type here
      oxygen: decimal;
      carbonDioxide: decimal
    }
  
  let makeTile(tileType, pos) = 
    {
      tileType=tileType; 
      pos=pos; 
      heat=defaultHeat; 
      oxygen=0.7M; 
      carbonDioxide=0.3M
    } 
   

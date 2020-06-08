namespace MattEland.FSharpStarship.Logic

open Positions

module World =

  type GameObjectType =
    | Player

  type GameObject =
    {
      pos: Pos;
      objectType: GameObjectType;
    }

  let private defaultHeat: decimal = 0M;

  type TileType =
    | Floor
    | Wall
    | WallLeft
    | WallRight
    //| Pipe
    //| Pump
    //| Tank
    //| Cable
    //| Door
    //| Capacitor
    //| PowerPlant
    //| HeatSink
    //| Vent
    //| Crate
    //| Cabinet
    //| Workstation
    //| Terminal
    //| Table
    //| Chair
    //| Toilet
    //| Hydroponics
    //| FoodPrep

  type Tile = 
    {
      tileType: TileType; 
      pos: Pos; 
      heat: decimal; 
      // TODO: Probably should have a gas composition type here
      oxygen: decimal;
      carbonDioxide: decimal
    }
  
  type GameWorld = 
    {
      tiles: List<Tile>;
      objects: List<GameObject>;
    }

  let getTile(world: GameWorld, pos: Pos): Option<Tile> = world.tiles |> List.tryFind(fun t -> t.pos = pos)
  let getObjects(world: GameWorld, pos: Pos): List<GameObject> = world.objects |> List.where(fun o -> o.pos = pos)

  let makeTile(tileType, pos) = 
    {
      tileType=tileType; 
      pos=pos; 
      heat=defaultHeat; 
      oxygen=0.7M; 
      carbonDioxide=0.3M;
    } 
   

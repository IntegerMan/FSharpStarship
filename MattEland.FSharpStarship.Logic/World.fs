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

  let private defaultHeat: decimal = 0.2M;
  let private defaultOxygen: decimal = 0.7M;
  let private defaultCO2: decimal = 0.3M;

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

  let getTileOxygen(world: GameWorld, pos: Pos): decimal =
    match getTile(world, pos) with
      | Some t -> t.oxygen
      | None -> 0M

  type TileContext = 
    {
      tile: Tile;
      up: Option<Tile>;
      down: Option<Tile>;
      left: Option<Tile>;
      right: Option<Tile>;
    }

  let getContext(world: GameWorld, tile: Tile): TileContext =
    {
      tile=tile;
      up=getTile(world, offset(tile.pos, 0, -1));
      down=getTile(world, offset(tile.pos, 0, 1));
      left=getTile(world, offset(tile.pos, -1, 0));
      right=getTile(world, offset(tile.pos, 1, 0));
    }

  let makeTile(tileType, pos) = 
    {
      tileType=tileType; 
      pos=pos; 
      heat=defaultHeat; 
      oxygen=0.1M * (pos.x |> decimal);  // TODO: defaultOxygen
      carbonDioxide=defaultCO2;
    } 
   
  let private replaceTileIfMatch(tile: Tile, testPos: Pos, newTile: Tile): Tile =
    if tile.pos = testPos then
      newTile
    else
      tile

  let replaceTile(world: GameWorld, pos: Pos, newTile: Tile): GameWorld =
    {world with tiles=world.tiles |> List.map(fun t -> replaceTileIfMatch(t, pos, newTile)) }
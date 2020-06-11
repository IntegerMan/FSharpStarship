namespace MattEland.FSharpStarship.Logic

open Utils
open Positions

module World =

  type GameObjectType =
    | Player

  type GameObject =
    {
      pos: Pos;
      objectType: GameObjectType;
    }

  type TileType =
    | Floor
    | Wall
    | WallLeft
    | WallRight
    | Space

  type Tile = 
    {
      tileType: TileType; 
      pos: Pos; 
      // TODO: It'd be nice to be able to have a collection of gasses, potentially
      heat: decimal; 
      oxygen: decimal;
      carbonDioxide: decimal;
      power: decimal;
    }
  
  type GameWorld = 
    {
      tiles: List<Tile>;
      objects: List<GameObject>;
    }

  let getTile pos world = world.tiles |> List.tryFind(fun t -> t.pos = pos)
  let getObjects pos world = world.objects |> List.where(fun o -> o.pos = pos)

  type Gas =
    | Oxygen
    | CarbonDioxide
    | Heat
    | Electrical

  let getTileGas(tile: Tile, gas: Gas): decimal =
      match gas with
      | Oxygen -> tile.oxygen
      | CarbonDioxide -> tile.carbonDioxide
      | Heat -> tile.heat
      | Electrical -> tile.power

  let retainsGas tileType = tileType <> TileType.Space

  let setTileGas gas requestedValue tile =
    if retainsGas tile.tileType then
      // Ensure we don't outside the 0 - 1 range
      let value = clamp(requestedValue, 0M, 1M)

      // Set the relevant gas
      match gas with
      | Oxygen -> {tile with oxygen=value}
      | CarbonDioxide -> {tile with carbonDioxide=value}
      | Heat -> {tile with heat=value}
      | Electrical -> {tile with power=value}
    else
      tile // Tiles that don't retain gasses should not be altered

  let getGasByPos(world: GameWorld, pos: Pos, gas: Gas): decimal = 
    let tile = getTile pos world
    if tile.IsSome then
      getTileGas(tile.Value, gas)
    else
      0M

  let private getDefaultGas tileType gas =
    match tileType with
    | Floor ->
      match gas with
      | Gas.Oxygen -> 0.7M
      | Gas.CarbonDioxide -> randomizer.NextDouble() |> decimal // 0.3M
      | Gas.Heat -> randomizer.NextDouble() |> decimal // 0.3M
      | Gas.Electrical -> 0M
    | _ -> 0M

  let makeTile(tileType, pos) = 
    {
      tileType=tileType; 
      pos=pos; 
      heat=getDefaultGas tileType Gas.Heat
      oxygen=getDefaultGas tileType Gas.Oxygen
      carbonDioxide=getDefaultGas tileType Gas.CarbonDioxide;
      power=getDefaultGas tileType Gas.Electrical
    } 
   
  let private replaceTileIfMatch(tile: Tile, testPos: Pos, newTile: Tile): Tile =
    if tile.pos = testPos then
      newTile
    else
      tile

  let replaceTile(world: GameWorld, pos: Pos, newTile: Tile): GameWorld =
    {world with tiles=world.tiles |> List.map(fun t -> replaceTileIfMatch(t, pos, newTile)) }

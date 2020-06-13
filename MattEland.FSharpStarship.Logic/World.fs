namespace MattEland.FSharpStarship.Logic

open Utils
open Positions

module World =

  type GameObjectType =
    | Astronaut
    | AirScrubber

  type GameObject =
    {
      Pos: Pos
      ObjectType: GameObjectType
    }

  type TileType =
    | Floor
    | Wall
    | WallLeft
    | WallRight
    | Space

  type Tile = 
    {
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      // TODO: It'd be nice to be able to have a collection of gasses, potentially
      Heat: decimal 
      Oxygen: decimal
      CarbonDioxide: decimal
      Power: decimal
    }
  
  type GameWorld = 
    {
      Tiles: List<Tile>
      Objects: List<GameObject>
    }

  let getTile pos world = world.Tiles |> List.find(fun t -> t.Pos = pos)
  let tryGetTile pos world = world.Tiles |> List.tryFind(fun t -> t.Pos = pos)
  let getObjects pos world = world.Objects |> List.where(fun o -> o.Pos = pos)

  type Gas =
    | Oxygen
    | CarbonDioxide
    | Heat
    | Electrical

  let pressurizedGasses = [Oxygen; CarbonDioxide]

  let getTileGas gas tile =
      match gas with
      | Oxygen -> tile.Oxygen
      | CarbonDioxide -> tile.CarbonDioxide
      | Heat -> tile.Heat
      | Electrical -> tile.Power

  let hasGas gas tile = tile |> getTileGas gas > 0M

  let retainsGas tileType = tileType <> TileType.Space

  let setTileGas gas requestedValue tile =
    if retainsGas tile.TileType then
      // Ensure we don't go negative
      let value = System.Math.Max(0M, requestedValue)

      // Set the relevant gas
      match gas with
      | Oxygen -> {tile with Oxygen=value; Pressure=tile.Pressure - tile.Oxygen + value}
      | CarbonDioxide -> {tile with CarbonDioxide=value; Pressure=tile.Pressure - tile.CarbonDioxide + value}
      | Heat -> {tile with Heat=value}
      | Electrical -> {tile with Power=value}
    else
      tile // Tiles that don't retain gasses should not be altered

  let modifyTileGas gas delta tile =
    let oldValue = tile |> getTileGas gas
    let newValue = oldValue + delta
    tile |> setTileGas gas newValue

  let getTopMostGas tile = pressurizedGasses |> List.find(fun gas -> tile |> hasGas gas)
  let tryGetTopMostGas tile = pressurizedGasses |> List.tryFind(fun gas -> tile |> hasGas gas)

  let getGasByPos(world: GameWorld, pos: Pos, gas: Gas): decimal = world |> getTile pos |> getTileGas gas

  let private getDefaultGas tileType gas =
    match tileType with
    | Floor ->
      match gas with
      | Gas.Oxygen -> 0.7M
      | Gas.CarbonDioxide -> randomDecimal() // 0.3M
      | Gas.Heat -> randomDecimal() // 0.3M
      | Gas.Electrical -> 0M
    | _ -> 0M

  let makeTile(tileType, pos) = 
    let tile = {
      TileType=tileType; 
      Pos=pos; 
      Heat=getDefaultGas tileType Gas.Heat
      Oxygen=getDefaultGas tileType Gas.Oxygen
      CarbonDioxide=getDefaultGas tileType Gas.CarbonDioxide;
      Power=getDefaultGas tileType Gas.Electrical
      Pressure=0M
    }
    {tile with Pressure=tile.Oxygen + tile.CarbonDioxide} // TODO: This is all sorts of WTF. I need a way to calc pressure and bake it in to the tile. Probably a Gasses type.
   
  let private replaceTileIfMatch(tile: Tile, testPos: Pos, newTile: Tile): Tile =
    if tile.Pos = testPos then
      newTile
    else
      tile

  let replaceTile pos newTile world = {world with Tiles=world.Tiles |> List.map(fun t -> replaceTileIfMatch(t, pos, newTile)) }

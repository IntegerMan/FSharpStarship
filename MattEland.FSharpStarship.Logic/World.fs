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

  type TileGas =
    {
      Oxygen: decimal
      CarbonDioxide: decimal
      Heat: decimal 
      Power: decimal
    }

  type Tile = 
    {
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
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

  let calculatePressure gasses = gasses.Oxygen + gasses.CarbonDioxide

  let getTileGas gas tile =
      match gas with
      | Oxygen -> tile.Gasses.Oxygen
      | CarbonDioxide -> tile.Gasses.CarbonDioxide
      | Heat -> tile.Gasses.Heat
      | Electrical -> tile.Gasses.Power

  let hasGas gas tile = tile |> getTileGas gas > 0M

  let retainsGas tileType = tileType <> TileType.Space

  let private setTileGas (gas: Gas) (requestedValue: decimal) (tile: Tile): Tile =
    if retainsGas tile.TileType then
      // Ensure we don't go negative
      let value = System.Math.Max(0M, requestedValue)

      // Set the relevant gas
      let gasses = 
        match gas with
        | Oxygen -> {tile.Gasses with Oxygen=value}
        | CarbonDioxide -> {tile.Gasses with CarbonDioxide=value}
        | Heat -> {tile.Gasses with Heat=value}
        | Electrical -> {tile.Gasses with Power=value}

      {tile with Gasses=gasses; Pressure=gasses |> calculatePressure}
    else
      tile // Tiles that don't retain gasses should not be altered

  let modifyTileGas gas delta tile: Tile =
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

  let defaultGasses tileType =
    {
      Oxygen=getDefaultGas tileType Oxygen
      CarbonDioxide=getDefaultGas tileType CarbonDioxide
      Heat=getDefaultGas tileType Heat
      Power=getDefaultGas tileType Electrical
    }

  let private getDefaultTileGasses tileType =
    {
      Heat=getDefaultGas tileType Gas.Heat
      Oxygen=getDefaultGas tileType Gas.Oxygen
      CarbonDioxide=getDefaultGas tileType Gas.CarbonDioxide;
      Power=getDefaultGas tileType Gas.Electrical
    }

  let makeTile(tileType, pos) = 
    let gasses = getDefaultTileGasses tileType
    {
      TileType=tileType
      Pos=pos
      Gasses=gasses
      Pressure=gasses |> calculatePressure
    }
   
  let makeTileWithGasses tileType pos gasses = 
    let tile = makeTile(tileType, pos)
    {tile with Gasses=gasses; Pressure=gasses |> calculatePressure}

  let private replaceTileIfMatch(tile: Tile, testPos: Pos, newTile: Tile): Tile =
    if tile.Pos = testPos then
      newTile
    else
      tile

  let replaceTile pos newTile world = {world with Tiles=world.Tiles |> List.map(fun t -> replaceTileIfMatch(t, pos, newTile)) }

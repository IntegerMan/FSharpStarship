namespace MattEland.FSharpStarship.Logic

open Tiles
open Gasses

module TileGas =

  let getTileGas gas tile =
      match gas with
      | Oxygen -> tile.Gasses.Oxygen
      | CarbonDioxide -> tile.Gasses.CarbonDioxide
      | Heat -> tile.Gasses.Heat
      | Electrical -> tile.Gasses.Power
      | Nitrogen -> tile.Gasses.Nitrogen

  let hasGas gas tile = tile |> getTileGas gas > 0M

  let private setTileGas (gas: Gas) (requestedValue: decimal) (tile: Tile): Tile =
    if tile.Flags.RetainsGas then
      // Ensure we don't go negative
      let value = System.Math.Max(0M, requestedValue)

      // Set the relevant gas
      let gasses = 
        match gas with
        | Oxygen -> {tile.Gasses with Oxygen=value}
        | CarbonDioxide -> {tile.Gasses with CarbonDioxide=value}
        | Heat -> {tile.Gasses with Heat=value}
        | Electrical -> {tile.Gasses with Power=value}
        | Nitrogen -> {tile.Gasses with Nitrogen=value}

      {tile with Gasses=gasses; Pressure=gasses |> calculatePressure}
    else
      tile // Tiles that don't retain gasses should not be altered

  let modifyTileGas gas delta tile =
    let oldValue = tile |> getTileGas gas
    let newValue = oldValue + delta
    tile |> setTileGas gas newValue

  let getTopMostGas tile = pressurizedGasses |> List.find(fun gas -> tile |> hasGas gas)
  let getBottomMostGas tile = pressurizedGasses |> List.rev |> List.find(fun gas -> tile |> hasGas gas)
  let tryGetTopMostGas tile = pressurizedGasses |> List.tryFind(fun gas -> tile |> hasGas gas)
  let tryGetBottomMostGas tile = pressurizedGasses |> List.rev |> List.tryFind(fun gas -> tile |> hasGas gas)

  let private getDefaultGas gas =
    match gas with
    | Gas.Oxygen -> 0.2M
    | Gas.CarbonDioxide -> 0.1M
    | Gas.Heat -> 0.3M
    | Gas.Electrical -> 0M
    | Nitrogen -> 0.8M

  let defaultGasses =
    {
      Oxygen=getDefaultGas Oxygen
      CarbonDioxide=getDefaultGas CarbonDioxide
      Heat=getDefaultGas Heat
      Power=getDefaultGas Electrical
      Nitrogen=getDefaultGas Nitrogen
    }

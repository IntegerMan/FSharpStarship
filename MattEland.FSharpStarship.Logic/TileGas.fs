namespace MattEland.FSharpStarship.Logic

open Tiles
open Gasses

module TileGas =

  let getGas gas gasContainer =
      match gas with
      | Oxygen -> gasContainer.Oxygen
      | CarbonDioxide -> gasContainer.CarbonDioxide
      | Heat -> gasContainer.Heat
      | Electrical -> gasContainer.Power
      | Nitrogen -> gasContainer.Nitrogen

  let hasGas gas tile = tile.Gasses |> getGas gas > 0M

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
    let oldValue = tile.Gasses |> getGas gas
    let newValue = oldValue + delta
    tile |> setTileGas gas newValue

  let getTopMostGas tile = pressurizedGasses |> List.find(fun gas -> tile |> hasGas gas)
  let getBottomMostGas tile = pressurizedGasses |> List.rev |> List.find(fun gas -> tile |> hasGas gas)
  let tryGetTopMostGas tile = pressurizedGasses |> List.tryFind(fun gas -> tile |> hasGas gas)
  let tryGetBottomMostGas tile = pressurizedGasses |> List.rev |> List.tryFind(fun gas -> tile |> hasGas gas)

namespace MattEland.FSharpStarship.Logic

open Tiles
open Gasses
open GameObjects

module TileGas =

  let getGas gas gasContainer =
      match gas with
      | Oxygen -> gasContainer.Oxygen
      | CarbonDioxide -> gasContainer.CarbonDioxide
      | Heat -> gasContainer.Heat
      | Power -> gasContainer.Power
      | Nitrogen -> gasContainer.Nitrogen

  let setGas gas value gasContainer =
      match gas with
      | Oxygen -> {gasContainer with Oxygen = value}
      | CarbonDioxide -> {gasContainer with CarbonDioxide = value}
      | Heat -> {gasContainer with Heat = value}
      | Power -> {gasContainer with Power = value}
      | Nitrogen -> {gasContainer with Nitrogen = value}
  
  let hasGas gas tile = tile.Gasses |> getGas gas > 0M

  let setTileGas (gas: Gas) (requestedValue: decimal) (tile: Tile): Tile =
    if tile.Flags.RetainsGas then
      // Ensure we don't go negative
      let value = System.Math.Max(0M, requestedValue)

      // Set the relevant gas
      let gasses = tile.Gasses |> setGas gas value
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

  type GasContext = {
    SourceGas: TileGas
    DestinationGas: TileGas
  }
  
  let bidirectionalEqualizeGas gas context =
    let sourceAmount = context.SourceGas |> getGas gas
    let destinationAmount = context.DestinationGas |> getGas gas
    let average = System.Math.Round((sourceAmount + destinationAmount) / 2M, 2)
    {context with SourceGas = (context.SourceGas |> setGas gas average); DestinationGas = (context.DestinationGas |> setGas gas average)} 
    
  let bidirectionalEqualize gasContext =    
    spreadableGasses
    |> List.fold(fun currentContext gas -> currentContext |> bidirectionalEqualizeGas gas) gasContext

  let unidirectionalShiftGas gas amount context =
    let sourceAmount = context.SourceGas |> getGas gas
    let destinationAmount = context.DestinationGas |> getGas gas
    { context with
        SourceGas = (context.SourceGas |> setGas gas (sourceAmount - amount))
        DestinationGas = (context.DestinationGas |> setGas gas (destinationAmount + amount))
    }  
  
  let unidirectionalShift amount gasContext =
    spreadableGasses
    |> List.fold(fun currentContext gas -> currentContext |> unidirectionalShiftGas gas amount) gasContext


  let getPipeGas gas object =
    match object.ObjectType with
    | AirPipe pipeGas -> pipeGas |> getGas gas
    | _ -> 0M

  let getPipeGasFromTile gas tile =
    match tile.Objects |> List.tryFind(isAirPipe) with
    | Some pipe -> pipe |> getPipeGas gas
    | None -> 0M
    
  let getPipeGasses pipe =
    match pipe.ObjectType with
    | AirPipe pipeGas -> pipeGas
    | _ -> emptyGasses
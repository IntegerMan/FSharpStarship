namespace MattEland.FSharpStarship.Logic

open MattEland.FSharpStarship.Logic.GameObjects
open World
open Tiles
open Gasses
open TileGas
open Contexts

module SimulateGasses =
  
  let private shiftGas(source: Tile) (dest: Tile) gas tiles =
    let gasContext = {SourceGas = source.Gasses; DestinationGas = dest.Gasses}
    let newGasses = gasContext |> unidirectionalShiftGas gas 0.01M

    tiles
    |> replaceTile {source with Gasses=newGasses.SourceGas; Pressure=calculatePressure newGasses.SourceGas}
    |> replaceTile {dest with Gasses=newGasses.DestinationGas; Pressure=calculatePressure newGasses.DestinationGas}
  
  let private tryFindTargetForGasSpread gas pos tiles =
    let tile = tiles |> getTile pos
    
    tile
    |> getContext tiles
    |> getPresentNeighbors
    |> List.filter(fun n -> canGasFlowFrom tile n gas)
    |> List.sortBy(fun n -> getGas gas n.Gasses)
    |> List.tryHead

  let rec private equalizeTileGas pos gas world =
    let target = tryFindTargetForGasSpread gas pos world
    match target with
    | Some neighbor ->
      let tile = world |> getTile pos
      world
      |> shiftGas tile neighbor gas
      |> equalizeTileGas tile.Pos gas // May be more gas to shift
    | None ->
      world
      
  let flowGasFromPipeToPipe sourceTile destTile gas world =
    let sourcePipe = sourceTile.Objects |> List.find(isAirPipe)
    let destPipe = destTile.Objects |> List.find(isAirPipe)
    
    let context = {
      SourceGas = sourcePipe |> getPipeGasses
      DestinationGas = destPipe |> getPipeGasses 
    }
    
    let newContext = context |> unidirectionalShiftGas gas 0.01M 
    
    let newSource = sourceTile |> replaceObject sourcePipe {ObjectType=AirPipe newContext.SourceGas}
    let newDest = destTile |> replaceObject destPipe {ObjectType=AirPipe newContext.DestinationGas}
    
    world
    |> replaceTile newSource
    |> replaceTile newDest
      
  let rec simulateAirPipeGas pos gas world =
    let tile = world |> getTile pos
    let tileGas = tile |> getPipeGasFromTile gas
    
    if tileGas < 0.01M then
      world
    else
      let target =
        world
        |> getNeighboringTilesWithPipes tile
        |> List.filter(fun t -> t |> getPipeGasFromTile gas < tileGas)
        |> List.sortBy(fun t -> t |> getPipeGasFromTile gas)
        |> List.tryHead
        
      match target with
      | None ->
          world
      | Some targetTile ->
          world
          |> flowGasFromPipeToPipe tile targetTile gas
          |> simulateAirPipeGas pos gas // Go recursive in case more gas needs to flow 
    
  let simulateAirPipe tile world =
    spreadableGasses
    |> List.fold(fun currentWorld gas -> simulateAirPipeGas tile.Pos gas currentWorld) world

  let simulateVent tile world =
    let pipeOption = tile.Objects |> List.tryFind(isAirPipe)
    match pipeOption with
    | None ->
      world
    | Some pipe ->
      match pipe.ObjectType with
      | AirPipe pipeGas ->
      
        let context = {SourceGas = tile.Gasses; DestinationGas = pipeGas}  
        let gasSolution = bidirectionalEqualize context
        
        let newTile =
          {tile with Gasses = gasSolution.SourceGas; Pressure = (gasSolution.SourceGas |> calculatePressure)}
          |> replaceObject pipe {pipe with ObjectType = AirPipe gasSolution.DestinationGas}
              
        world
        |> replaceTile newTile 
      | _ -> // This will not happen; I'm only using match here to get the specific gasses
        world      
  
  let simulateTileGas pos world = 
    spreadableGasses 
    |> List.fold(fun newWorld gas -> newWorld |> equalizeTileGas pos gas) world

  let convertTileGas amount gasSource gasGen tile =
    let currentGas = tile.Gasses |> getGas gasSource 
    if currentGas >= amount then
      tile |> modifyTileGas gasSource -amount |> modifyTileGas gasGen amount
    else if currentGas > 0M then
      tile |> modifyTileGas gasSource -currentGas |> modifyTileGas gasGen currentGas
    else
      tile

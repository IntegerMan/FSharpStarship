namespace MattEland.FSharpStarship.Logic

open MattEland.FSharpStarship.Logic.GameObjects
open World
open Tiles
open Gasses
open TileGas
open Contexts

module SimulateGasses =

  let private shiftGas (source: Tile) (dest: Tile) gas tiles =
    tiles
    |> replaceTile (source |> modifyTileGas gas -0.01M)
    |> replaceTile (dest |> modifyTileGas gas 0.01M)

  let isClosedDoor (object: GameObject) =
    match object.ObjectType with
    | Door(IsOpen = isOpen) -> not isOpen
    | _ -> false
    
  let hasClosedDoor tile =
    tile.Objects |> List.exists(fun o -> o |> isClosedDoor)
    
  let gasCanFlowInto tile =
    let hasGasBlocker = tile |> hasClosedDoor
    not hasGasBlocker && not tile.Flags.BlocksGas
  
  let canGasFlowFrom tileSource tileDestination gas =
    let destGas = getGas gas tileDestination.Gasses
    let sourceGas = getGas gas tileSource.Gasses
    sourceGas > 0M && gasCanFlowInto tileDestination && (sourceGas > destGas || (tileSource |> hasClosedDoor))
  
  let private tryFindTargetForGasSpread gas pos tiles =
    let tile = tiles |> getTile pos
    
    tile |> getContext tiles
    |> getPresentNeighbors
    |> List.filter(fun n -> canGasFlowFrom tile n gas)
    |> List.sortBy(fun n -> getGas gas n.Gasses)
    |> List.tryHead

  let rec private equalizeTileGas pos gas (tiles: List<Tile>) =
    let target = tryFindTargetForGasSpread gas pos tiles
    match target with
    | None -> tiles
    | Some neighbor ->
      let tile = tiles |> getTile pos
      tiles
      |> shiftGas tile neighbor gas
      |> equalizeTileGas tile.Pos gas // May be more gas to shift

  let getNeighboringTilesWithPipes tile world =
      tile
      |> getContext world
      |> getPresentNeighbors
      |> List.filter(fun t -> t.Objects |> List.exists(isAirPipe))

  let simulateVentAndAirPipe ventTile pipeTile world =
    let pipe = pipeTile.Objects |> List.find(isAirPipe)
    match pipe.ObjectType with
    | AirPipe gasses ->
      let newGasses = {gasses with Oxygen = gasses.Oxygen + 0.01M} // TODO: Flow it away from the ventTile
      world |> replaceTile (pipeTile |> replaceObject pipe {pipe with ObjectType = AirPipe newGasses})
    | _ -> // This will not happen; I'm only using match here to get the specific gasses
      world      
  
  let simulateVent tile world =
    world
    |> getNeighboringTilesWithPipes tile
    |> List.fold(fun newWorld neighbor -> newWorld |> simulateVentAndAirPipe tile neighbor) world
    
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

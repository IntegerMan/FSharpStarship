namespace MattEland.FSharpStarship.Logic

open World
open Tiles
open Gasses
open TileGas
open Contexts

module SimulateGasses =

  let canGasFlowInto tile =
    match tile.TileType with
      | Floor | Space -> true
      | Door (IsOpen=true) -> true
      | Door (IsOpen=false) -> false
      | _ -> false

  let private shiftGas (source: Tile) (dest: Tile) gas world =
    world
    |> replaceTile source.Pos (modifyTileGas gas -0.01M source)
    |> replaceTile dest.Pos (modifyTileGas gas 0.01M dest)

  let private tryFindTargetForGasSpread gas pos world =
    let tile = world |> getTile pos
    let currentGas = tile |> getTileGas gas
    getContext(world, tile)
    |> getPresentNeighbors
    |> List.filter(fun n -> canGasFlowInto n && getTileGas gas n < currentGas)
    |> List.sortBy(fun n -> getTileGas gas n)
    |> List.tryHead

  let rec private equalizeTileGas pos gas world =
    let target = tryFindTargetForGasSpread gas pos world
    match target with
    | None -> world
    | Some neighbor ->
      let tile = world |> getTile pos
      world 
      |> shiftGas tile neighbor gas
      |> equalizeTileGas tile.Pos gas // May be more gas to shift

  let simulateTileGas pos world = 
    spreadableGasses 
    |> List.fold(fun newWorld gas -> newWorld |> equalizeTileGas pos gas) world

  let convertTileGas amount gasSource gasGen tile =
    if tile |> getTileGas gasSource >= amount then
      tile |> modifyTileGas gasSource -amount |> modifyTileGas gasGen amount
    else
      tile

namespace MattEland.FSharpStarship.Logic

open World
open Tiles
open Gasses
open TileGas
open Contexts

module SimulateGasses =

  let private shiftGas (source: Tile) (dest: Tile) gas tiles =
    tiles
    |> replaceTile (modifyTileGas gas -0.01M source)
    |> replaceTile (modifyTileGas gas 0.01M dest)

  let private tryFindTargetForGasSpread gas pos tiles =
    let tile = tiles |> getTile pos
    let currentGas = tile |> getTileGas gas
    tile |> getContext tiles
    |> getPresentNeighbors
    |> List.filter(fun n -> not n.Flags.BlocksGas && getTileGas gas n < currentGas)
    |> List.sortBy(fun n -> getTileGas gas n)
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

  let simulateTileGas pos world = 
    spreadableGasses 
    |> List.fold(fun newWorld gas -> newWorld |> equalizeTileGas pos gas) world

  let convertTileGas amount gasSource gasGen tile =
    if tile |> getTileGas gasSource >= amount then
      tile |> modifyTileGas gasSource -amount |> modifyTileGas gasGen amount
    else
      tile

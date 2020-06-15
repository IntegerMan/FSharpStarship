namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open Tiles
open TileGas
open GameObjects

module World =

  type GameWorld = 
    {
      Tiles: List<Tile>
      Objects: List<GameObject>
    }

  let getTile pos world = world.Tiles |> List.find(fun t -> t.Pos = pos)
  let tryGetTile pos world = world.Tiles |> List.tryFind(fun t -> t.Pos = pos)
  let getObjects pos world = world.Objects |> List.where(fun o -> o.Pos = pos)

  let getGasByPos(world: GameWorld, pos: Pos, gas: Gas): decimal = world |> getTile pos |> getTileGas gas

  let makeTile tileType pos = 
    let gasses = getDefaultTileGasses tileType
    {
      TileType=tileType
      Pos=pos
      Gasses=gasses
      Pressure=gasses |> calculatePressure
    }
   
  let makeTileWithGasses tileType pos gasses = 
    let tile = makeTile tileType pos
    {tile with Gasses=gasses; Pressure=gasses |> calculatePressure}

  let private replaceTileIfMatch(tile: Tile, testPos: Pos, newTile: Tile): Tile =
    if tile.Pos = testPos then
      newTile
    else
      tile

  let replaceTile pos newTile world = {world with Tiles=world.Tiles |> List.map(fun t -> replaceTileIfMatch(t, pos, newTile)) }

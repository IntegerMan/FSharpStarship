module TestHelpers

open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Tiles
open MattEland.FSharpStarship.Logic.TileGas

let standardGas = defaultGasses TileType.Floor

let makeFloorTile pos objects gasses = makeTileWithGasses TileType.Floor pos objects gasses
let makeWallTile pos objects = makeTileWithGasses TileType.Wall pos objects (defaultGasses TileType.Wall)
let makeSpaceTile pos objects = makeTileWithGasses TileType.Space pos objects (defaultGasses TileType.Space)

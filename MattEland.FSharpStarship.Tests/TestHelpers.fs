module TestHelpers

open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Tiles
open MattEland.FSharpStarship.Logic.TileGas

let standardGas = defaultGasses TileType.Floor

let makeFloorTile pos gasses = makeTileWithGasses TileType.Floor pos gasses
let makeWallTile pos = makeTileWithGasses TileType.Wall pos (defaultGasses TileType.Wall)
let makeSpaceTile pos = makeTileWithGasses TileType.Space pos (defaultGasses TileType.Space)

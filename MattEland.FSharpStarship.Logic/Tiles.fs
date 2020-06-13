namespace MattEland.FSharpStarship.Logic

open Utils
open Positions
open Gasses

module Tiles =

  type TileType =
    | Floor
    | Wall
    | WallLeft
    | WallRight
    | Space

  type Tile = 
    {
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
    }

  let retainsGas tileType = tileType <> TileType.Space

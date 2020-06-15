namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses

module Tiles =

  type TileType =
    | Floor
    | Wall
    | Door of IsOpen:bool
    | Space

  type Tile = 
    {
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
    }

  let retainsGas tileType = tileType <> TileType.Space

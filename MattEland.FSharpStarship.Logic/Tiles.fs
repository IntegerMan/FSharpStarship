namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses

module Tiles =

  type TileType =
    | Floor
    | Wall
    | Space
    | Carpet

  type TileArt =
    {
      TileFile: string
      X: int
      Y: int
      Width: int
      Height: int
    }

  type Tile = 
    {
      Art: TileArt option
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
    }

  let retainsGas tileType = tileType <> TileType.Space

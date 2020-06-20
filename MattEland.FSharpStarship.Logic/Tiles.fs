namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open GameObjects

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
      Objects: List<GameObject> // TODO: Seq
    }

  let retainsGas tileType = tileType <> TileType.Space

namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open GameObjects

module Tiles =

  type TileType =
    | Floor
    | Wall
    | AirPipe
    | WaterPipe
    | Space

  type TileArt =
    {
      TileFile: string
      X: int
      Y: int
      Width: int
      Height: int
      ZIndex: int
    }

  type Tile = 
    {
      Art: List<TileArt> // TODO: Seq
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
      Objects: List<GameObject> // TODO: Seq
    }

  let retainsGas tileType = tileType <> TileType.Space

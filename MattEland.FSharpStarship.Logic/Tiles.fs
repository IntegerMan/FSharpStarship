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

  let emptyArt =
    {
      TileFile="Images/Test.png"
      X=0
      Y=0
      Width=32
      Height=32
    }

  type Tile = 
    {
      Art: TileArt
      TileType: TileType
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
      Objects: List<GameObject> // TODO: Seq
    }

  let retainsGas tileType = tileType <> TileType.Space

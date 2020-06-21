namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open GameObjects

module Tiles =

  type TileFlags =
    {
      RetainsGas: bool
      BlocksGas: bool
      BlocksMovement: bool
    }

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
      Flags: TileFlags
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
      Objects: List<GameObject> // TODO: Seq
    }

  let spaceFlags = {RetainsGas=false; BlocksGas=false; BlocksMovement=false}
  let wallFlags = {RetainsGas=false; BlocksGas=true; BlocksMovement=true}
  let tileFlags = {RetainsGas=true; BlocksGas=false; BlocksMovement=false}
  let doorFlags = {RetainsGas=true; BlocksGas=true; BlocksMovement=false}

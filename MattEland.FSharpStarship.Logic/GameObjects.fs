namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open Tiles
open TileGas

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber

  type GameObject =
    {
      Pos: Pos
      ObjectType: GameObjectType
    }
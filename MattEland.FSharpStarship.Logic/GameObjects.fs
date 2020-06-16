namespace MattEland.FSharpStarship.Logic

open Positions

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber
    | Door of IsOpen:bool * IsHorizontal:bool
    | Bed of IsLeft:bool
    | SideTable
    | Shelf of IsLeft:bool
    | Desk of IsLeft:bool

  type GameObject =
    {
      Pos: Pos
      ObjectType: GameObjectType
    }
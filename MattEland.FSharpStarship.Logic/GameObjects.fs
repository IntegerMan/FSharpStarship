namespace MattEland.FSharpStarship.Logic

open Positions

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber
    | Door of IsOpen:bool * IsHorizontal:bool

  type GameObject =
    {
      ObjectType: GameObjectType
    }
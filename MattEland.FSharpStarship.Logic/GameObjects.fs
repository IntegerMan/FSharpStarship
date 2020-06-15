namespace MattEland.FSharpStarship.Logic

open Positions

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber

  type GameObject =
    {
      Pos: Pos
      ObjectType: GameObjectType
    }
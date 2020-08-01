namespace MattEland.FSharpStarship.Logic

open Positions

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber
    | Vent
    | EngineIntake
    | WaterTank
    | Plant
    | Door of IsOpen:bool * IsHorizontal:bool

  type GameObject =
    {
      ObjectType: GameObjectType
    }
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
    
  let findClosedDoor (objects: GameObject list): GameObject option =
      objects
      |> List.tryPick(fun o ->
                        match o.ObjectType with
                        | Door(IsOpen = false) -> Some o
                        | _ -> None
                      )
  let toggleDoorOpen objectType =
      match objectType with
      | Door(isOpen, isHorizontal) -> Door(IsOpen = not isOpen, IsHorizontal = isHorizontal)
      | _ -> objectType
namespace MattEland.FSharpStarship.Logic

open MattEland.FSharpStarship.Logic.Gasses

module GameObjects =

  type GameObjectType =
    | Astronaut
    | AirScrubber
    | Vent
    | EngineIntake
    | WaterTank
    | Plant
    | AirPipe of TileGas
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
  let toggleDoorOpen gameObject =
      match gameObject.ObjectType with
      | Door(isOpen, isHorizontal) -> {gameObject with ObjectType=Door(IsOpen = not isOpen, IsHorizontal = isHorizontal)}
      | _ -> gameObject
      
  let isAirPipe gameObject =
    match gameObject.ObjectType with
    | AirPipe _ -> true
    | _ -> false
    
  let isClosedDoor (object: GameObject) =
    match object.ObjectType with
    | Door(IsOpen = isOpen) -> not isOpen
    | _ -> false

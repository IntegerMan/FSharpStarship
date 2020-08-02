namespace MattEland.FSharpStarship.Logic

open World
open Tiles
open Gasses
open GameObjects
open SimulateGasses

module Simulations =

  let engineOxygenIntake = 0.1M
  let humanOxygenIntake = 0.1M
  let scrubberCO2Intake = 0.1M
  let plantCO2Intake = 0.02M
  let doorAutoCloseDistance = 2.5
  
  let private simulatePerson tile world =
    let newTile = tile |> convertTileGas humanOxygenIntake Gas.Oxygen Gas.CarbonDioxide
    world |> replaceTile newTile

  let private simulateEngineIntake tile world =
    let newTile = tile |> convertTileGas engineOxygenIntake Gas.Oxygen Gas.CarbonDioxide
    world |> replaceTile newTile

  let private simulateAirScrubber tile world =
    let newTile = tile |> convertTileGas scrubberCO2Intake Gas.CarbonDioxide Gas.Oxygen
    world |> replaceTile newTile

  let private simulatePlant tile world =
    let newTile = tile |> convertTileGas plantCO2Intake Gas.CarbonDioxide Gas.Oxygen
    world |> replaceTile newTile
    
  let private simulateDoor (gameObject:GameObject) isOpen tile world =
    match isOpen with
    | false ->
        world
    | true ->
        let nearbyObjects = world |> getObjectsInRadius tile doorAutoCloseDistance
        let nearbyPlayer = nearbyObjects |> List.tryFind(fun o -> o.ObjectType = Astronaut)
        match nearbyPlayer with
        | Some _ ->
          world
        | None ->
          let closedDoor = toggleDoorOpen gameObject
          let newTile = tile |> replaceObject gameObject closedDoor
          world |> replaceTile newTile
          
  let private simulateObject obj tile world =
    match obj.ObjectType with
    | Astronaut -> simulatePerson tile world
    | EngineIntake -> simulateEngineIntake tile world
    | AirScrubber -> simulateAirScrubber tile world
    | Plant -> simulatePlant tile world
    | Vent -> simulateVent tile world
    | Door(isOpen, _) -> simulateDoor obj isOpen tile world
    | _ -> world

  let private simulateObjects objects tile world = 
    objects 
    |> List.fold(fun newWorld obj -> newWorld |> simulateObject obj tile) world

  let simulateTile tile tiles = 
    tiles
    |> simulateObjects tile.Objects tile
    |> simulateTileGas tile.Pos

  let simulate tiles = tiles |> List.fold(fun newTiles tile -> newTiles |> simulateTile tile) tiles
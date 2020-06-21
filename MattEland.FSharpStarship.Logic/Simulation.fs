namespace MattEland.FSharpStarship.Logic

open World
open Tiles
open Gasses
open GameObjects
open SimulateGasses

module Simulations =

  let humanOxygenIntake = 0.1M
  let scrubberCO2Intake = 0.1M
  
  let private simulatePerson tile world =
    let newTile = tile |> convertTileGas humanOxygenIntake Gas.Oxygen Gas.CarbonDioxide
    world |> replaceTile tile.Pos newTile

  let private simulateAirScrubber tile world =
    let newTile = tile |> convertTileGas scrubberCO2Intake Gas.CarbonDioxide Gas.Oxygen
    world |> replaceTile tile.Pos newTile

  let private simulateObject obj tile world =
    match obj.ObjectType with
    | Astronaut -> simulatePerson tile world
    | AirScrubber -> simulateAirScrubber tile world
    | _ -> world

  let private simulateObjects objects tile world = 
    objects 
    |> List.fold(fun newWorld obj -> newWorld |> simulateObject obj tile) world

  let simulateTile tile world = 
    world 
    |> simulateObjects tile.Objects tile
    |> simulateTileGas tile.Pos

  let simulate(world: GameWorld): GameWorld =
    world.Tiles 
    |> List.fold(fun newWorld tile -> newWorld |> simulateTile tile) world
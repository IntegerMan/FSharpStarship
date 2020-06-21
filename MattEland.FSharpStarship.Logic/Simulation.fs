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
    world |> replaceTile newTile

  let private simulateAirScrubber tile world =
    let newTile = tile |> convertTileGas scrubberCO2Intake Gas.CarbonDioxide Gas.Oxygen
    world |> replaceTile newTile

  let private simulateObject obj tile world =
    match obj.ObjectType with
    | Astronaut -> simulatePerson tile world
    | AirScrubber -> simulateAirScrubber tile world
    | _ -> world

  let private simulateObjects objects tile world = 
    objects 
    |> List.fold(fun newWorld obj -> newWorld |> simulateObject obj tile) world

  let simulateTile tile tiles = 
    tiles
    |> simulateObjects tile.Objects tile
    |> simulateTileGas tile.Pos

  let simulate tiles = tiles |> List.fold(fun newTiles tile -> newTiles |> simulateTile tile) tiles
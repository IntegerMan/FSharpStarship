namespace MattEland.FSharpStarship.Logic

open Utils
open Positions

module Gasses =

  type TileGas =
    {
      Oxygen: decimal
      CarbonDioxide: decimal
      Heat: decimal 
      Power: decimal
    }

  type Gas =
    | Oxygen
    | CarbonDioxide
    | Heat
    | Electrical

  let pressurizedGasses = [Oxygen; CarbonDioxide]
  let spreadableGasses = [Heat; Oxygen; CarbonDioxide; Electrical]

  let calculatePressure gasses = gasses.Oxygen + gasses.CarbonDioxide


namespace MattEland.FSharpStarship.Logic

module Gasses =

  type TileGas =
    {
      Nitrogen: decimal
      Oxygen: decimal
      CarbonDioxide: decimal
      Heat: decimal 
      Power: decimal
    }

  let emptyGasses = {
      Nitrogen = 0M
      Oxygen = 0M
      CarbonDioxide = 0M
      Heat = 0M
      Power = 0M
    }

  type Gas =
    | Nitrogen
    | Oxygen
    | CarbonDioxide
    | Heat
    | Electrical

  let pressurizedGasses = [Nitrogen; Oxygen; CarbonDioxide]
  let spreadableGasses = [Heat; Electrical] @ pressurizedGasses

  let calculatePressure gasses = gasses.Oxygen + gasses.CarbonDioxide + gasses.Nitrogen


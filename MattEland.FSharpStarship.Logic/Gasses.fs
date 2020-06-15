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

  type Gas =
    | Nitrogen
    | Oxygen
    | CarbonDioxide
    | Heat
    | Electrical

  let pressurizedGasses = [Nitrogen; Oxygen; CarbonDioxide]
  let spreadableGasses = [Heat; Electrical] @ pressurizedGasses

  let calculatePressure gasses = gasses.Oxygen + gasses.CarbonDioxide + gasses.Nitrogen


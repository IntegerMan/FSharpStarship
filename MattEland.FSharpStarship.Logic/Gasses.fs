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
    | Power

  let pressurizedGasses = [Nitrogen; Oxygen; CarbonDioxide]
  let spreadableGasses = [Heat; Power] @ pressurizedGasses

  let calculatePressure gasses = gasses.Oxygen + gasses.CarbonDioxide + gasses.Nitrogen

  let private getDefaultGas gas =
    match gas with
    | Gas.Oxygen -> 0.2M
    | Gas.CarbonDioxide -> 0.1M
    | Gas.Heat -> 0.3M
    | Gas.Power -> 0M
    | Nitrogen -> 0.8M

  let defaultGasses =
    {
      Oxygen=getDefaultGas Oxygen
      CarbonDioxide=getDefaultGas CarbonDioxide
      Heat=getDefaultGas Heat
      Power=getDefaultGas Power
      Nitrogen=getDefaultGas Nitrogen
    }
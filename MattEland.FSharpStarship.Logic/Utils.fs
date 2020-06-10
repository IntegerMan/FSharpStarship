namespace MattEland.FSharpStarship.Logic

open System

module Utils =

  let randomizer = new Random()

  let truncateToTwoDecimalPlaces(number) = Math.Truncate(number * 100M) / 100M

  let clamp(value:decimal, min: decimal, max: decimal): decimal = Math.Min(max, Math.Max(min, value))

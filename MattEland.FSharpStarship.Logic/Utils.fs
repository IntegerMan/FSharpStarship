namespace MattEland.FSharpStarship.Logic

open System

module Utils =

  let randomizer = new Random()

  let truncateToTwoDecimalPlaces(number) = Math.Truncate(number * 100M) / 100M

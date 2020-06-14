namespace MattEland.FSharpStarship.Logic

open System

module Utils =

  let truncateToTwoDecimalPlaces number = Math.Truncate(number * 100M) / 100M

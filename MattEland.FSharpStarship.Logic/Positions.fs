namespace MattEland.FSharpStarship.Logic

module Positions =

  type Pos = {X: int; Y: int}

  let offset xDelta yDelta pos = {X=pos.X + xDelta; Y = pos.Y + yDelta}

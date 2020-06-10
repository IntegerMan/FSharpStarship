namespace MattEland.FSharpStarship.Logic

module Positions =

  type Pos = {x: int; y: int}

  let offset xDelta yDelta pos = {x=pos.x + xDelta; y = pos.y + yDelta}

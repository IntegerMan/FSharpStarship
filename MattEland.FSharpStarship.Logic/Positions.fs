namespace MattEland.FSharpStarship.Logic

module Positions =

  type Pos = {X: int; Y: int}
  let pos x y = {X=x; Y=y}

  let offset xDelta yDelta pos = {X=pos.X + xDelta; Y = pos.Y + yDelta}


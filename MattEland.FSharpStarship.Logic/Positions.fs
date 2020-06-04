namespace MattEland.FSharpStarship.Logic

module Positions =

  type Pos = {x: int; y: int}

  let offset(pos: Pos, xDelta, yDelta): Pos = {x=pos.x + xDelta; y = pos.y + yDelta}

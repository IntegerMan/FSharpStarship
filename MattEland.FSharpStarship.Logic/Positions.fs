namespace MattEland.FSharpStarship.Logic

module Positions =

  type Direction =
    | Up
    | Right
    | Down
    | Left

  type Pos = {X: int; Y: int}
  let pos x y = {X=x; Y=y}

  let offset xDelta yDelta pos = {X=pos.X + xDelta; Y = pos.Y + yDelta}

  let offsetDir direction pos =
    match direction with 
    | Up -> pos |> offset 0 -1
    | Down -> pos |> offset 0 1
    | Left -> pos |> offset -1 0
    | Right -> pos |> offset 1 0
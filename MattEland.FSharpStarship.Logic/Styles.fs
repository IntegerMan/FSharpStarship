namespace MattEland.FSharpStarship.Logic

open Common

module Styles =

  type RGB = {r: byte; g: byte; b: byte}

  let getBackgroundColor (tile: Tile): RGB =
    match tile.tileType with
    | Floor -> {r=86uy; g=86uy; b=128uy}
    | Wall -> {r=64uy; g=64uy; b=84uy}
    | _ -> {r=255uy; g=0uy; b=255uy}

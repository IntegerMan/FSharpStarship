namespace MattEland.FSharpStarship.Logic

open Positions
open World

module Simulations =

  let private simulateTile(tile: Tile, world: GameWorld): Tile = {tile with oxygen=tile.oxygen - 0.1M}

  let simulate(world: GameWorld): GameWorld = { world with tiles=world.tiles |> List.map(fun t -> simulateTile(t, world)) }

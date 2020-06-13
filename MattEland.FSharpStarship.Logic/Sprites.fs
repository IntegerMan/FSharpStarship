namespace MattEland.FSharpStarship.Logic

open GameObjects
open Tiles

module Sprites =

  type SpriteInfo = {Image: string; X: int; Y: int; Width: int; Height: int; ZIndex: int}

  let private getZIndex (tileType: TileType): int =
    match tileType with
    | Wall -> 1
    | _ -> 0

  let private getTileX (tileType: TileType): int =
    match tileType with
    | Wall -> 6
    | WallLeft | WallRight -> 7
    | Floor -> 0
    | Space -> 3

  let private getTileY (tileType: TileType): int =
    match tileType with
    | Wall -> 0
    | WallLeft | WallRight -> 2
    | Floor -> 11
    | Space -> 10

  let getTileSpriteInfo (tileType: TileType): SpriteInfo =
    {
      Image="tileset2.png"
      X=getTileX(tileType);
      Y=getTileY(tileType);
      Width=32;
      Height=32;
      ZIndex=getZIndex(tileType)
    }

  let getObjectSpriteInfo (object: GameObject): SpriteInfo =
    let defaultArt = { Image="tileset2.png"; X=0; Y=0; Width=32; Height=32; ZIndex=1}
    match object.ObjectType with
    | Astronaut -> { defaultArt with Image="Astronaut.png"; X=1; Y=0; ZIndex=5}
    | AirScrubber -> { defaultArt with X=7; Y=11}
    
namespace MattEland.FSharpStarship.Logic

open GameObjects
open Tiles

module Sprites =

  type SpriteInfo = {
    Image: string
    X: int
    Y: int
    OffsetX: int
    OffsetY: int
    Width: int
    Height: int
    ZIndex: int
  }

  let private getZIndex (tileType: TileType): int =
    match tileType with
    | Wall -> 1
    | Door _ -> 2
    | _ -> 0

  let private getTileX (tileType: TileType): int =
    match tileType with
    | Wall -> 6
    | Door _ -> 4
    | Floor -> 0
    | Space -> 3

  let private getTileY (tileType: TileType): int =
    match tileType with
    | Wall -> 0
    | Door (IsOpen=true) -> 5
    | Door (IsOpen=false) -> 4
    | Floor -> 11
    | Space -> 10

  let getTileSpriteInfo (tileType: TileType): SpriteInfo =
    {
      Image="tileset2.png"
      X=getTileX(tileType);
      Y=getTileY(tileType);
      Width=32;
      Height=32;
      OffsetX=0;
      OffsetY=0;
      ZIndex=getZIndex(tileType)
    }

  let getObjectSpriteInfo (object: GameObject): SpriteInfo =
    let defaultArt = { Image="tileset2.png"; X=0; Y=0; Width=32; Height=32; ZIndex=1; OffsetX=0; OffsetY=0}
    match object.ObjectType with
    | Astronaut -> { defaultArt with Image="Matt.png"; X=0; Y=0; Width=48; Height=96; ZIndex=5; OffsetY = -2}
    | AirScrubber -> { defaultArt with X=7; Y=11}
    
namespace MattEland.FSharpStarship.Logic

open GameObjects
open Tiles

module Sprites =

  type SpriteLocationType =
    | Cell
    | AbsolutePosition

  type SpriteInfo = {
    Image: string
    LocationType: SpriteLocationType
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
    | _ -> 0

  let private getTileX (tileType: TileType): int =
    match tileType with
    | Wall -> 6
    | Floor -> 0
    | Space -> 3

  let private getTileY (tileType: TileType): int =
    match tileType with
    | Wall -> 0
    | Floor -> 11
    | Space -> 10

  let getTileSpriteInfo (tileType: TileType): SpriteInfo =
    {
      Image="tileset2.png"
      X=getTileX(tileType)
      Y=getTileY(tileType)
      LocationType=Cell
      Width=32
      Height=32
      OffsetX=0
      OffsetY=0
      ZIndex=getZIndex(tileType)
    }

  let getObjectSpriteInfo (object: GameObject): SpriteInfo =
    let defaultArt = { Image="tileset2.png"; X=0; Y=0; Width=32; Height=32; ZIndex=1; OffsetX=0; OffsetY=0;LocationType=Cell}
    match object.ObjectType with
    | Astronaut -> { defaultArt with Image="Matt.png"; X=0; Y=0; Width=48; Height=96; ZIndex=5; OffsetY = -2}
    | AirScrubber -> { defaultArt with X=7; Y=11}
    | Bed (IsLeft=true) -> {defaultArt with Image="Mid-TownB.png"; LocationType=AbsolutePosition; X=39; Y=79; Width=20; Height=28}
    | Bed (IsLeft=false) -> {defaultArt with Image="Mid-TownB.png"; LocationType=AbsolutePosition; X=60; Y=79; Width=20; Height=28}
    | Door (IsHorizontal=true; IsOpen=true) -> {defaultArt with X=6; Y=5; ZIndex=2}
    | Door (IsHorizontal=true; IsOpen=false) -> {defaultArt with X=5; Y=5; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=false) -> {defaultArt with X=4; Y=4; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=true) -> {defaultArt with X=4; Y=5; ZIndex=2}
    
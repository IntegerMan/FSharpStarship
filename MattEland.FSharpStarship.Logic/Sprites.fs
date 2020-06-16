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
    | Desk (IsLeft=true) -> {defaultArt with Image="Mid-TownD.png"; LocationType=AbsolutePosition; X=98; Y=226; Width=30; Height=30}
    | Desk (IsLeft=false) -> {defaultArt with Image="Mid-TownD.png"; LocationType=AbsolutePosition; X=98; Y=196; Width=30; Height=30}
    | SideTable -> {defaultArt with Image="Mid-TownB.png"; LocationType=AbsolutePosition; X=112; Y=85; Width=16; Height=20}
    | Shelf (IsLeft=true) -> {defaultArt with Image="Hospital.png"; LocationType=AbsolutePosition; X=576; Y=917; Width=72; Height=108}
    | Shelf (IsLeft=false) -> {defaultArt with Image="Hospital.png"; LocationType=AbsolutePosition; X=647; Y=917; Width=72; Height=108}
    | Door (IsHorizontal=true; IsOpen=true) -> {defaultArt with X=6; Y=5; ZIndex=2}
    | Door (IsHorizontal=true; IsOpen=false) -> {defaultArt with X=5; Y=5; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=false) -> {defaultArt with X=4; Y=4; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=true) -> {defaultArt with X=4; Y=5; ZIndex=2}
    | _ -> {defaultArt with Image="Mid-TownB.png"; Width=16; Height=16}
    
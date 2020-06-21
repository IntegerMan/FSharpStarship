namespace MattEland.FSharpStarship.Logic

open GameObjects
open Tiles

module Sprites =

  let sciWhiteTile = {
    TileFile="images/tileset2.png"
    X=0
    Y=0
    Width=32
    Height=32
    ZIndex = 2
  }

  let getObjectSpriteInfo (object: GameObject): TileArt =
    match object.ObjectType with
    | Astronaut -> { TileFile="images/Matt.png"; X=0; Y=0; Width=48; Height=96; ZIndex=30}
    | AirScrubber -> { sciWhiteTile with X=7; Y=11}
    | Door (IsHorizontal=true; IsOpen=true) -> {sciWhiteTile with X=6; Y=5}
    | Door (IsHorizontal=true; IsOpen=false) -> {sciWhiteTile with X=5; Y=5}
    | Door (IsHorizontal=false; IsOpen=false) -> {sciWhiteTile with X=4; Y=4}
    | Door (IsHorizontal=false; IsOpen=true) -> {sciWhiteTile with X=4; Y=5}

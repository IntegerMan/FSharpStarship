namespace MattEland.FSharpStarship.Logic

open Common
open Microsoft.FSharp.Reflection
open System

module View =

  type CurrentOverlay =
    | None = 0
    | Thermal = 1
    | Gas = 2
    | Fluid = 3
    | Electrical = 4

  type AppView = {
    overlay: CurrentOverlay
  }

  let getDefaultAppView() = {
    overlay=CurrentOverlay.None; 
  }

  let changeOverlay(view: AppView, newOverlay: CurrentOverlay): AppView = {view with overlay = newOverlay}    

  type RGB = {r: byte; g: byte; b: byte}

  let rgb (r, g, b): RGB = {r = byte r; g = byte g; b = byte b}

  let getTileColor (tileType: TileType): RGB =
    match tileType with
    | Floor -> rgb(86, 86, 128)
    | Wall -> rgb(64, 64, 84)
    | _ -> rgb(255, 0, 255) // Magenta for high visibility

  let getBackgroundColor (tile: Tile, view: AppView): RGB =
    match view.overlay with
    | CurrentOverlay.Thermal -> rgb((tile.heat * 255M) |> Math.Round |> int, 0, 0)
    | _ -> getTileColor(tile.tileType)

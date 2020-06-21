namespace MattEland.FSharpStarship.Logic

open Tiles

module View =

  type CurrentOverlay =
    | None = 0
    | Heat = 1
    | Nitrogen = 2
    | Oxygen = 3
    | CarbonDioxide = 4
    | Electrical = 5
    | Fluid = 6
    | Pressure = 7
    | Particles = 8

  type AppView = {
    Overlay: CurrentOverlay;
    Zoom: int;
  }

  let getDefaultAppView() = {
    Overlay=CurrentOverlay.None; 
    Zoom=1;
  }

  let changeOverlay(view: AppView, newOverlay: CurrentOverlay): AppView = {view with Overlay = newOverlay}    

  type RGB = {R: byte; G: byte; B: byte; T: byte}

  let rgbt (r, g, b, t): RGB = {R = byte r; G = byte g; B = byte b; T = byte t}
  let rgb (r, g, b): RGB = rgbt(r,g,b, 255)
  let transparent = rgbt(255, 255, 255, 0)

  let private getGradedColor percent alpha = 
    let value = (System.Math.Min(1M, percent) * 255M) |> System.Math.Round |> int
    rgbt(value, value, value, alpha)

  let getBackgroundColor (tile: Tile, view: AppView): RGB =
    match view.Overlay with
    | CurrentOverlay.Oxygen -> getGradedColor tile.Gasses.Oxygen 100
    | CurrentOverlay.Nitrogen -> getGradedColor tile.Gasses.Nitrogen 100
    | CurrentOverlay.CarbonDioxide -> getGradedColor tile.Gasses.CarbonDioxide 100
    | CurrentOverlay.Heat -> getGradedColor tile.Gasses.Heat 100
    | CurrentOverlay.Pressure -> getGradedColor (tile.Pressure / 3.0M) 100
    | _ -> transparent

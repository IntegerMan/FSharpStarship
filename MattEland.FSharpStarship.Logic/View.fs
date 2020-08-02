namespace MattEland.FSharpStarship.Logic

open Tiles

module View =

  type CurrentOverlay =
    | None = 0
    | Heat = 1
    | Nitrogen = 2
    | Oxygen = 3
    | CarbonDioxide = 4
    | Power = 5
    | Fluid = 6
    | Pressure = 7
    | Particles = 8

  type AppView = {
    Overlay: CurrentOverlay;
    Zoom: int;
  }

  let getDefaultAppView() = {
    Overlay=CurrentOverlay.Particles; 
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
    
  let private getColorOnRange value max =
    let percent = value / max
    getGradedColor percent 100

  let getBackgroundColor (tile: Tile, view: AppView): RGB =
    match view.Overlay with
    | CurrentOverlay.Oxygen -> getColorOnRange tile.Gasses.Oxygen 0.3M
    | CurrentOverlay.Nitrogen -> getColorOnRange tile.Gasses.Nitrogen 0.8M
    | CurrentOverlay.CarbonDioxide -> getColorOnRange tile.Gasses.CarbonDioxide 0.3M
    | CurrentOverlay.Heat -> getColorOnRange tile.Gasses.Heat 1M
    | CurrentOverlay.Pressure -> getGradedColor (tile.Pressure / 3.0M) 100
    | _ -> transparent

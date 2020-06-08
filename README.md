# Modeling a Starship in F#
Code for an upcoming talk on Modeling a Starship in F#. Intended as a fun introduction to the F# programming language.

## Credits

### Code
All code written by Matt Eland (@IntegerMan)

### Art
Artwork commercially purchased from DithArt and used in open source software with permission.

Visit https://dithart.itch.io/ for more information on DithArt's assets. Artwork cannot be reused, repurposed, or redistributed without their express written consent.

## Tasks

- [x] Create a simple 1-room space ship
- [x] Overlay mode
- [ ] Overlay for Heat
- [ ] Overlay for Oxygen
- [ ] Overlay for Carbon Dioxide
- [ ] Overlay for Fluids
- [ ] Overlay for Electrical
- [x] Tile Graphics
- [x] Render actors
- [x] Next turn button
- [ ] Simulate when next turn is clicked
- [x] Add a human actor
- [ ] Convert O2 to CO2 near human
- [ ] Allow the player to move the human actor
- [ ] Create a multi-room space ship

## Systems Modeled

### Environmental

- Power Generation
- Power Storage
- Power Consumption
- Heat
- Environment Gas Composition (O2 / CO2)
- Storage Tanks
- Fluid Pipes / Tanks (Water, Untreated Water, Fuel, etc.)
- Water Recycling
- Air Recycling

### Crew

- Crew Locations
- Crew Tasks
- Crew Efficiency
- Crew Fatigue

### Operational

- Storage
- Current Vector
- Thrust
- Fuel
- Weapons?
- Communications?
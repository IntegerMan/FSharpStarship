using System;
using System.Windows.Media;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class GasParticleViewModel : ViewModelBase
    {
        private static readonly Random _randomizer = new Random();

        private readonly Gasses.Gas _gas;

        public GasParticleViewModel(TileViewModel tile, Gasses.Gas gas)
        {
            _gas = gas;
            Tile = tile;
        }

        public TileViewModel Tile { get; }

        public int PosX => Tile.PosX + (_randomizer.Next(0, 32) * Tile.AppView.Zoom);
        public int PosY => Tile.PosY + (_randomizer.Next(0, 32) * Tile.AppView.Zoom);

        public int Width => Tile.AppView.Zoom * 2;
        public int Height => Tile.AppView.Zoom * 2;

        public decimal Opacity => Tile.AppView.Overlay.Equals(View.CurrentOverlay.Particles) ? 1M : 0M;

        public Brush Background
        {
            get
            {
                if (_gas.Equals(Gasses.Gas.Oxygen))
                {
                    return Brushes.DodgerBlue;
                }

                if (_gas.Equals(Gasses.Gas.CarbonDioxide))
                {
                    return Brushes.Salmon;
                }

                if (_gas.Equals(Gasses.Gas.Nitrogen))
                {
                    return Brushes.GhostWhite;
                }

                return Brushes.Magenta;
            }
        }

        public void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Opacity));
        }
    }
}

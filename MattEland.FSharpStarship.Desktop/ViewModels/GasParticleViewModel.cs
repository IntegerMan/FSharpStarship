using System;
using System.Collections.Generic;
using System.Text;
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

        public decimal Opacity => 0.42M;

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

                return Brushes.Magenta;
            }
        }
    }
}

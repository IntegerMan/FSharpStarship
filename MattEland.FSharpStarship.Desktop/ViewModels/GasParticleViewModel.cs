using System;
using System.Windows.Media;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class GasParticleViewModel : ImageViewModel
    {
        private static readonly Random _randomizer = new Random();

        private readonly Gasses.Gas _gas;

        public GasParticleViewModel(TileViewModel tile, Gasses.Gas gas) : base(Brushes.Transparent, 40, 0.5M)
        {
            _gas = gas;
            Tile = tile;
        }

        public TileViewModel Tile { get; }

        public override int PosX => _randomizer.Next(0, Tile.ImageWidth - 1);
        public override int PosY => _randomizer.Next(0, Tile.ImageHeight - 1);

        public override int Width => 2;
        public override int Height => 2;

        public override Brush Background
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
    }
}

using System.Collections.Generic;
using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : WorldEntityViewModel
    {
        public Tiles.Tile Tile { get; }

        public TileViewModel(Tiles.Tile tile, MainViewModel mainViewModel) : base(mainViewModel)
        {
            Tile = tile;
        }

        public override void HandleOverlayChanged()
        {
            base.HandleOverlayChanged();

            OnPropertyChanged(nameof(OverlayBrush));
        }

        public override Sprites.SpriteInfo SpriteInfo => Sprites.getTileSpriteInfo(Tile.TileType);

        public override string ToolTip => $"{Tile.TileType}\nPos: ({Tile.Pos.X}, {Tile.Pos.Y})\nOxygen: {Tile.Gasses.Oxygen}\nCO2: {Tile.Gasses.CarbonDioxide}\nHeat: {Tile.Gasses.Heat}\nPressure: {Tile.Pressure}";

        public override int PosX => Tile.Pos.X * TileWidth;
        public override int PosY => Tile.Pos.Y * TileHeight;

        public Brush OverlayBrush
        {
            get
            {
                if (AppView.Overlay == View.CurrentOverlay.None) return Brushes.Transparent;

                var color = CalculateColor();
                Brush brush = new SolidColorBrush(color);

                brush.Freeze(); // TODO: Introduce a brush factory to store unique / reused brushes

                return brush;
            }
        }

        public override Brush Background
        {
            get
            {
                if (Tile.TileType.Equals(Tiles.TileType.Space)) return Brushes.Transparent;

                return BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);
            return Color.FromRgb(rgb.R, rgb.G, rgb.B);
        }

        public IList<GasParticleViewModel> BuildParticles()
        {
            List<GasParticleViewModel> particles = new List<GasParticleViewModel>();

            AddGasParticles(particles, Gasses.Gas.CarbonDioxide);
            AddGasParticles(particles, Gasses.Gas.Oxygen);

            return particles;
        }

        private void AddGasParticles(ICollection<GasParticleViewModel> particles, Gasses.Gas gasType)
        {
            const decimal gasThreshhold = 0.1M;

            decimal gasLevel = TileGas.getTileGas(gasType, Tile);
            while (gasLevel >= gasThreshhold)
            {
                particles.Add(new GasParticleViewModel(this, gasType));
                gasLevel -= gasThreshhold;
            }
        }
    }
}
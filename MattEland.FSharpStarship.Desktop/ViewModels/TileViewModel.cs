using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override string ToolTip
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(Tile.TileType.ToString());
                sb.AppendLine($"Pos: {Tile.Pos.X},{Tile.Pos.Y}");

                Gasses.spreadableGasses.ToList().ForEach(g => { sb.AppendLine($"{g}: {TileGas.getTileGas(g, Tile)}"); });

                sb.AppendLine($"Pressure: {Tile.Pressure}");

                return sb.ToString();
            }
        }

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

            if (rgb.T == 0) return Colors.Transparent;

            return Color.FromArgb(rgb.T, rgb.R, rgb.G, rgb.B);
        }

        public IList<GasParticleViewModel> BuildParticles()
        {
            var particles = new List<GasParticleViewModel>();

            Gasses.pressurizedGasses.ToList().ForEach(g => AddGasParticles(particles, g));

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
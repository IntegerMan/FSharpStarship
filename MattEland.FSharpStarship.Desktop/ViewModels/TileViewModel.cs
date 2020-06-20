using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            RebuildImages();
        }

        public override void HandleOverlayChanged()
        {
            base.HandleOverlayChanged();

            OnPropertyChanged(nameof(OverlayBrush));
            RebuildImages();
        }

        private void RebuildImages()
        {
            Images.Clear();
            
            Images.Add(new ImageViewModel(Background, 0));

            // TODO: Add layers

            // TODO: Add objects
            //Tile.Objects.Select(t => new GameObjectViewModel(t, MainVM)).ToList().ForEach(o => Objects.Add(o));
        }

        public ObservableCollection<ImageViewModel> Images { get; } = new ObservableCollection<ImageViewModel>();

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

                var art = Tile.Art.Value;
                string pathToCheck = "Images/";
                int index = art.TileFile.LastIndexOf(pathToCheck, StringComparison.InvariantCultureIgnoreCase);

                if (index > 0)
                {
                    string resourceFile = art.TileFile.Substring(index + pathToCheck.Length);

                    if (IsSupportedResourceFile(resourceFile))
                    {
                        var fakeSprite = new Sprites.SpriteInfo(resourceFile,
                                                                Sprites.SpriteLocationType.AbsolutePosition, art.X,
                                                                art.Y, 0, 0, art.Width, art.Height, 1);

                        return BrushHelpers.GetBrushFromSpriteInfo(fakeSprite);
                    }
                }

                return BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);
            }
        }

        private bool IsSupportedResourceFile(string resourceFile)
        {
            switch (resourceFile.ToLowerInvariant())
            {
                case "tileset1.png":
                case "tileset2.png":
                case "mid-towna5.png":
                case "mid-townb.png":
                case "mid-townc.png":
                case "mid-townd.png":
                case "tilea5_phc_store.png":
                    return true;
                default:
                    return false;
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
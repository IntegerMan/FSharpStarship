using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public int TileWidth => 32;
        public int TileHeight => 32;

        public View.AppView AppView => MainVM.AppView;

        public int ImageWidth => TileWidth;
        public int ImageHeight => TileHeight;

        public MainViewModel MainVM { get; }

        public Tiles.Tile Tile { get; }

        public TileViewModel([NotNull] Tiles.Tile tile, [NotNull] MainViewModel mainViewModel)
        {
            MainVM = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            Tile = tile ?? throw new ArgumentNullException(nameof(tile));

            RebuildImages();
        }

        public void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(ToolTip));

            RebuildImages();
        }

        private void RebuildImages()
        {
            Images.Clear();
            
            // Add layers
            Tile.Art.Select(a => new ImageViewModel(BrushHelpers.GetBrushFromArt(a), a.ZIndex))
                .ToList()
                .ForEach(i => Images.Add(i));

            // Add objects
            Tile.Objects
                .Select(Sprites.getObjectSpriteInfo)
                .Select(si => BrushHelpers.GetBrushFromArt(si, Stretch.Uniform))
                .Select(b => new ImageViewModel(b, 30))
                .ToList()
                .ForEach(i => Images.Add(i));

            // Add gas particles if needed
            if (AppView.Overlay == View.CurrentOverlay.Particles)
            {
                BuildParticles().ForEach(p => Images.Add(p));
            }

            // Add Overlay if needed
            if (AppView.Overlay != View.CurrentOverlay.None && AppView.Overlay != View.CurrentOverlay.Particles)
            {
                Images.Add(new ImageViewModel(BuildOverlayBrush(), 50, 0.5M));
            }
        }

        private Brush BuildOverlayBrush() => BrushHelpers.GetSolidColorBrush(CalculateColor());

        public ObservableCollection<ImageViewModel> Images { get; } = new ObservableCollection<ImageViewModel>();

        public string ToolTip
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"Pos: {Tile.Pos.X},{Tile.Pos.Y}");

                Gasses.spreadableGasses.ToList().ForEach(g => { sb.AppendLine($"{g}: {TileGas.getTileGas(g, Tile)}"); });

                sb.AppendLine($"Pressure: {Tile.Pressure}");

                return sb.ToString();
            }
        }

        public int PosX => Tile.Pos.X * TileWidth;
        public int PosY => Tile.Pos.Y * TileHeight;

        public Brush CompositeImage
        {
            get
            {
                var group = new DrawingGroup();
                group.Append();

                // Add layers
                var rect = new Rect(new Size(TileWidth, TileHeight));
                foreach (var imageSource in Tile.Art.Select(a => BrushHelpers.GetImageSourceFromArt(a)).ToList())
                {
                    var imageDrawing = new ImageDrawing(imageSource, rect);
                    imageDrawing.Freeze();

                    group.Children.Add(imageDrawing);
                }

                // Add objects
                foreach (var imageSource in Tile.Objects
                                                .Select(Sprites.getObjectSpriteInfo)
                                                .Select(si => BrushHelpers.GetImageSourceFromArt(si, Stretch.Uniform))
                                                .ToList())
                {
                    var imageDrawing = new ImageDrawing(imageSource, new Rect(new Size(imageSource.Width, imageSource.Height)));
                    imageDrawing.Freeze();

                    group.Children.Add(imageDrawing);
                }

                /*
                // Add gas particles if needed
                if (AppView.Overlay == View.CurrentOverlay.Particles)
                {
                    BuildParticles().ForEach(p => Images.Add(p));
                }

                // Add Overlay if needed
                if (AppView.Overlay != View.CurrentOverlay.None && AppView.Overlay != View.CurrentOverlay.Particles)
                {
                    Images.Add(new ImageViewModel(BuildOverlayBrush(), 50, 0.5M));
                }
                */

                var brush = new DrawingBrush(group);
                brush.Freeze();

                return brush;
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);

            return rgb.T == 0 
                ? Colors.Transparent 
                : Color.FromArgb(rgb.T, rgb.R, rgb.G, rgb.B);
        }

        private List<GasParticleViewModel> BuildParticles()
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
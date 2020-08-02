using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Desktop.Rendering;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        private Tiles.Tile _tile;
        public int TileWidth => 32;
        public int TileHeight => 32;

        public View.AppView AppView => MainVM.AppView;

        public int ImageWidth => TileWidth;
        public int ImageHeight => TileHeight;

        public MainViewModel MainVM { get; }

        public Tiles.Tile Tile
        {
            get => _tile;
            set
            {
                if (Equals(value, _tile)) return; // This line actually causes random things to remain stationary, causing some interesting visual effects
                _tile = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToolTip));
                OnPropertyChanged(nameof(PosX));
                OnPropertyChanged(nameof(PosY));
                Renderer?.RefreshRender();
            }
        }

        public TileViewModel([NotNull] Tiles.Tile tile, [NotNull] MainViewModel mainViewModel)
        {
            MainVM = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            Tile = tile ?? throw new ArgumentNullException(nameof(tile));
        }

        public void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(ToolTip));
            Renderer.RefreshRender();
        }

        public Brush BuildOverlayBrush() => BrushHelpers.GetSolidColorBrush(CalculateColor());
        public SolidColorBrush BuildPipeBrush() => BrushHelpers.GetSolidColorBrush(CalculatePipeColor());

        public string ToolTip
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Pos: {Tile.Pos.X},{Tile.Pos.Y}");

                Gasses.spreadableGasses.ToList().ForEach(g => { sb.AppendLine($"{g}: {TileGas.getGas(g, Tile.Gasses)}"); });

                sb.AppendLine($"Pressure: {Tile.Pressure}");
                
                foreach (var obj in Tile.Objects)
                {
                    sb.AppendLine(obj.ObjectType.ToString());
                }

                return sb.ToString();
            }
        }

        public int PosX => Tile.Pos.X * TileWidth;
        public int PosY => Tile.Pos.Y * TileHeight;
        public TileRendererHost Renderer { get; set; }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);

            return rgb.T == 0 
                ? Colors.Transparent 
                : Color.FromArgb(rgb.T, rgb.R, rgb.G, rgb.B);
        }

        private Color CalculatePipeColor()
        {
            var rgb = View.getPipeColor(Tile, AppView);

            return rgb.T == 0 
                ? Colors.Transparent 
                : Color.FromArgb(rgb.T, rgb.R, rgb.G, rgb.B);
        }        
        
        public List<GasParticleViewModel> BuildParticles()
        {
            var particles = new List<GasParticleViewModel>();

            Gasses.pressurizedGasses.ToList().ForEach(g => AddGasParticles(particles, g));

            return particles;
        }

        private void AddGasParticles(ICollection<GasParticleViewModel> particles, Gasses.Gas gasType)
        {
            // if (gasType == Gasses.Gas.Nitrogen) return;
            
            decimal gasThreshhold;
            if (gasType == Gasses.Gas.Nitrogen)
            {
                gasThreshhold = 0.1M;
            }
            else
            {
                gasThreshhold = 0.025M;
            }

            decimal gasLevel = TileGas.getGas(gasType, Tile.Gasses);
            while (gasLevel >= gasThreshhold)
            {
                particles.Add(new GasParticleViewModel(this, gasType));
                gasLevel -= gasThreshhold;
            }
        }
    }
}
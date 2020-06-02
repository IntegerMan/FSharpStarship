using System.Collections.Generic;
using System.Linq;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private string _selectedViewMode = "None";

        public MainViewModel()
        {
            Tiles = Common.getTiles().Select(t => new TileViewModel(t, this)).ToList();
        }

        public List<TileViewModel> Tiles { get; }

        public IEnumerable<string> ViewModes { get; } = new List<string> { "None", "Heat", "Air", "Power", "Fluid"};

        public string SelectedViewMode
        {
            get => _selectedViewMode;
            set
            {
                if (value == _selectedViewMode) return;

                _selectedViewMode = value;
                OnPropertyChanged();

                Tiles.ForEach(t => t.HandleOverlayChanged());
            }
        }
    }
}

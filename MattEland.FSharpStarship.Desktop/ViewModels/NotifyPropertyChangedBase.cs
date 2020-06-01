using System.ComponentModel;
using System.Runtime.CompilerServices;
using MattEland.FSharpStarship.Desktop.Annotations;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
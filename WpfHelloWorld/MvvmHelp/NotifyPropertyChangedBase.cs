using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfHelloWorld.MvvmHelp
{
    /// <summary>
    /// Trying to simplify the clumsy INotifyPropertyChanged garbage - don't put your other crap in here!
    /// </summary>
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        readonly ILogger? _logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public NotifyPropertyChangedBase(ILogger? logger = null)
        {
            _logger = logger;
        }

        protected virtual bool SetProperty<T>(ref T backingField, T newValue, [CallerMemberName] string? propName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                _logger?.LogDebug("No change in {propName} == {backingField}", propName, backingField);
                return false;
            }

            _logger?.LogDebug("Changing {propName} from {backingField} to {newValue}", propName, backingField, newValue);
            backingField = newValue;
            OnPropertyChanged(propName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

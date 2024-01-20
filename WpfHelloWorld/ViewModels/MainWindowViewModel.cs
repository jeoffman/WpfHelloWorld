using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace WpfHelloWorld.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        readonly ILoggerFactory _loggerFactory;
        readonly ILogger<MainWindowViewModel> _logger;


        public ICommand? SaveCommand { get; private set; }
        public ICommand? ImportCommand { get; private set; }


        public MainWindowViewModel(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<MainWindowViewModel>();
        }

        #region IDisposable
        public void Dispose()
        {
            
        }
        #endregion IDisposable

        #region Properties
        public int MainWindowHeight
        {
            get => _mainWindowHeight;
            set => SetProperty(ref _mainWindowHeight, value);
        }
        int _mainWindowHeight = 400;
        public int MainWindowWidth
        {
            get => _mainWindowWidth;
            set => SetProperty(ref _mainWindowWidth, value);
        }
        int _mainWindowWidth = 600;
        public int MainWindowLeft
        {
            get => _mainWindowLeft;
            set => SetProperty(ref _mainWindowLeft, value);
        }
        int _mainWindowLeft = 20;
        public int MainWindowTop
        {
            get => _mainWindowTop;
            set => SetProperty(ref _mainWindowTop, value);
        }
        int _mainWindowTop = 20;

        #endregion Properties

    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfHelloWorld.Models;
using WpfHelloWorld.MvvmHelp;
using WpfHelloWorld.Services;

namespace WpfHelloWorld.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        const string DefaultDocumentFileName = "HelloWorld.hello";

        readonly ILoggerFactory _loggerFactory;
        readonly ILogger<MainWindowViewModel> _logger;
        readonly HelloDocument _helloDocument;
        readonly IClickerService _clickerService;

        public string DocumentFileName { get; set; } = DefaultDocumentFileName;


        public ICommand? SaveCommand { get; private set; }
        public ICommand? ClickerCommand { get; private set; }


        public MainWindowViewModel(ILoggerFactory loggerFactory, HelloDocument helloDocument, IClickerService clickerService)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<MainWindowViewModel>();
            _helloDocument = helloDocument;
            _clickerService = clickerService;

            _logger.LogDebug("");

            SaveCommand = new DelegateCommand<MainWindowViewModel>(async (o) => { await SaveDocumentAsync(); });
            ClickerCommand = new DelegateCommand<MainWindowViewModel>(async (o) => { await ClickAsync(); });

            Task.Run(async () =>
                {
                    await LoadDocumentAsync();    //WPF async await much?
                })
                .Wait();
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
        int _mainWindowHeight;
        public int MainWindowWidth
        {
            get => _mainWindowWidth;
            set => SetProperty(ref _mainWindowWidth, value);
        }
        int _mainWindowWidth;
        public int MainWindowLeft
        {
            get => _mainWindowLeft;
            set => SetProperty(ref _mainWindowLeft, value);
        }
        int _mainWindowLeft;
        public int MainWindowTop
        {
            get => _mainWindowTop;
            set => SetProperty(ref _mainWindowTop, value);
        }
        int _mainWindowTop;

        public int ClickerCounter
        {
            get => _clickerCounter;
            set => SetProperty(ref _clickerCounter, value);
        }
        int _clickerCounter;
        #endregion Properties

        public async Task LoadDocumentAsync()
        {
            await _helloDocument.LoadAsync(DocumentFileName);

            MainWindowHeight = _helloDocument.MainWindowHeight;
            MainWindowWidth = _helloDocument.MainWindowWidth;
            MainWindowTop = _helloDocument.MainWindowTop;
            MainWindowLeft = _helloDocument.MainWindowLeft;

            ClickerCounter = _helloDocument.ClickCounts;
            _clickerService.SetClicks(ClickerCounter);
        }

        public async Task SaveDocumentAsync()
        {
            _helloDocument.MainWindowHeight = MainWindowHeight;
            _helloDocument.MainWindowWidth = MainWindowWidth;
            _helloDocument.MainWindowTop = MainWindowTop;
            _helloDocument.MainWindowLeft = MainWindowLeft;

            _helloDocument.ClickCounts = ClickerCounter;
            _helloDocument.SomeHello = DateTime.UtcNow.ToString("o");

            await _helloDocument.SaveAsync(DocumentFileName);
        }

        private async Task ClickAsync()
        {
            Mouse.OverrideCursor = Cursors.AppStarting;

            try
            {
                await _clickerService.ClickAsync();
                ClickerCounter = _clickerService.ClickCounter;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

    }
}

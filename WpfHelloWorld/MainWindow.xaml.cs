using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;
using WpfHelloWorld.ViewModels;
using WpfHelloWorld.Views;

namespace WpfHelloWorld
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider, MainWindowViewModel mainWindowViewModel)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();

            DataContext = mainWindowViewModel;
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HelpCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AboutDialog? dialog = _serviceProvider.GetService<AboutDialog>();
            if (dialog == null)
                throw new InvalidOperationException("No About for you!");
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}
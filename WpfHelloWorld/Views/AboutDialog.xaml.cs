using System.Windows;

namespace WpfHelloWorld.Views
{
    /// <summary> Shamelessly stolen from this guy:  https://blogs.msdn.microsoft.com/pedrosilva/2009/05/08/wpfaboutbox-dialog-layout-and-styles/
    /// tweaked via AboutAssemblyDataProvider
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog(AboutAssemblyDataProvider about)
        {
            InitializeComponent();

            DataContext = about;
        }
    }
}

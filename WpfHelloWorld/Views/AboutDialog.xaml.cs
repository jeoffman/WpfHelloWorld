using System;
using System.Diagnostics;
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

        private void hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe", "/e,/select," + e.Uri.AbsolutePath.Replace("/", "\\")));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            e.Handled = true;
        }
    }
}

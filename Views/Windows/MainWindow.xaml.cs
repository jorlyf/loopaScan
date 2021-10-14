using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace loopaScan.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RequestNavigateEventArgs e)
        {
            var site = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
            };
            Process.Start(site);
        }
    }
}

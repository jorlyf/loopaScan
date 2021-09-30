using System;
using System.Windows;
using Microsoft.Win32;

using loopaScan.Models;
using loopaScan.Infrastructure;

namespace loopaScan.Views.Windows
{
    public partial class CreateSessionWindow : Window
    {
        public Session Session;
        private string _SessionName;
        private string _SessionFileName;
        private string _SessionThreadsCount;
        public CreateSessionWindow()
        {
            InitializeComponent();
        }

        private void Create(object Sender, RoutedEventArgs E)
        {
            _SessionName = SessionName.Text;
            _SessionThreadsCount = SessionThreadsCount.Text;
            if (!string.IsNullOrEmpty(_SessionName) && !string.IsNullOrEmpty(_SessionThreadsCount) && !string.IsNullOrEmpty(_SessionFileName))
            {
                Session = new Session
                {
                    Name = _SessionName,
                    FileName = _SessionFileName,
                    ThreadsCount = Convert.ToInt32(_SessionThreadsCount),
                    ScannedIPsCount = 0,
                    ScannedSuccessIPsCount = 0
                };
                DialogResult = true;
            } else
            {
                MessageBox.Show("Сессия не была создана");
            }

        }
        private void OpenFile(object Sender, RoutedEventArgs E)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = ".txt|*.txt",
                InitialDirectory = Directories.IPfiles
            };

            if (ofd.ShowDialog() == false) return;

            _SessionFileName = System.IO.Path.GetFileName(ofd.FileName);
        }
    }
}

using System.Windows;


namespace loopaScan.Views.Windows
{
    public partial class CreateSessionWindow : Window
    {
        public CreateSessionWindow()
        {
            InitializeComponent();
        }

        private void Create(object Sender, RoutedEventArgs E)
        {
            MessageBox.Show("Создан");
        }
        public string FileName = "123.txt";
        private void OpenFile(object Sender, RoutedEventArgs E)
        {

        }
    }
}

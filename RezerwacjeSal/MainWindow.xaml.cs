using System.Windows;
using RezerwacjeSal.Views; // Upewnij się, że masz to dodane!

namespace RezerwacjeSal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // To musi być tutaj!
        }

        private void OpenTestWindow_Click(object sender, RoutedEventArgs e)
        {
            TestWindow testWindow = new TestWindow();
            testWindow.Show();
        }
    }
}

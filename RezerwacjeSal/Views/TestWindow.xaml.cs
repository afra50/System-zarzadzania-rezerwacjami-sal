using System.Windows;

namespace RezerwacjeSal.Views
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            DataContext = new TestViewModel(); // Powiązanie ViewModel z XAML
        }
    }
}

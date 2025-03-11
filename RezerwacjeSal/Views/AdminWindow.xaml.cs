using System.Windows;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Brak dostępu!");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Czyszczenie danych użytkownika
            SessionManager.UserName = null;
            SessionManager.UserEmail = null;
            SessionManager.UserRole = null;

            // Powrót do ekranu startowego
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

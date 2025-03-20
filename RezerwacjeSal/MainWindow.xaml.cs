using System.Diagnostics;
using System.Windows;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Logika interakcji dla MainWindow.xaml.
    /// Główne okno aplikacji z opcjami logowania i rejestracji.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Inicjalizacja komponentów
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zaloguj się".
        /// Otwiera okno logowania i zamyka bieżące okno.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(); // Tworzy okno logowania
            loginWindow.Show(); // Wyświetla okno logowania
            this.Close(); // Zamykamy obecne okno
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarejestruj się".
        /// Otwiera okno rejestracji i zamyka bieżące okno.
        /// </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(); // Tworzy okno rejestracji
            registerWindow.Show(); // Wyświetla okno rejestracji
            this.Close(); // Zamykamy obecne okno
        }
    }
}

using System.Windows;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Logika interakcji dla AdminWindow.xaml.
    /// Okno panelu administratora, umożliwia zarządzanie salami, rezerwacjami i wylogowanie.
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            // Sprawdzamy, czy użytkownik ma odpowiednie uprawnienia
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Brak dostępu!"); // Powiadomienie o braku dostępu
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Zamykamy okno administratora
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj salami".
        /// Otwiera okno zarządzania salami.
        /// </summary>
        private void ManageRooms_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomsWindow manageRoomsWindow = new ManageRoomsWindow(); // Tworzymy okno do zarządzania salami
            manageRoomsWindow.Show();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Rezerwacje".
        /// Otwiera okno zarządzania rezerwacjami.
        /// </summary>
        private void ManageReservations_Click(object sender, RoutedEventArgs e)
        {
            ManageReservationsWindow manageReservationsWindow = new ManageReservationsWindow(); // Tworzymy okno do zarządzania rezerwacjami
            manageReservationsWindow.Show();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Wyloguj".
        /// Wylogowuje użytkownika i zamyka okno.
        /// </summary>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Czyszczenie danych użytkownika
            SessionManager.UserName = null;
            SessionManager.UserEmail = null;
            SessionManager.UserRole = null;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Powrót do ekranu startowego
            this.Close(); // Zamykamy okno administratora
        }
    }
}


using System.Windows;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Logika interakcji dla ClientWindow.xaml.
    /// Okno panelu klienta, umożliwia rezerwowanie sal, przeglądanie rezerwacji i wylogowanie.
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
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

            // Powrót do ekranu startowego
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Zamykamy okno klienta
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Rezerwuj salę".
        /// Otwiera okno rezerwacji sali.
        /// </summary>
        private void ReserveRoom_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno rezerwacji sali
            ReserveRoomWindow reserveRoomWindow = new ReserveRoomWindow();
            reserveRoomWindow.ShowDialog();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Moje rezerwacje".
        /// Otwiera okno wyświetlające rezerwacje klienta.
        /// </summary>
        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno "Moje rezerwacje"
            MyReservationsWindow myReservationsWindow = new MyReservationsWindow();
            myReservationsWindow.ShowDialog();
        }
    }
}

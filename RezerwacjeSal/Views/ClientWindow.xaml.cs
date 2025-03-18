using System.Windows;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
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

        private void ReserveRoom_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno rezerwacji
            ReserveRoomWindow reserveRoomWindow = new ReserveRoomWindow();
            reserveRoomWindow.ShowDialog();
        }

        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno "Moje rezerwacje"
            MyReservationsWindow myReservationsWindow = new MyReservationsWindow();
            myReservationsWindow.ShowDialog();
        }

    }
}

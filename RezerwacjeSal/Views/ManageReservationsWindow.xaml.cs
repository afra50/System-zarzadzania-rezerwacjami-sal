using System.Windows;

namespace RezerwacjeSal.Views
{
    public partial class ManageReservationsWindow : Window
    {
        public ManageReservationsWindow()
        {
            InitializeComponent();
        }

        private void BrowseReservations_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno z rezerwacjami
            ReservationsListWindow reservationsListWindow = new ReservationsListWindow();
            reservationsListWindow.Show();
        }
    }
}

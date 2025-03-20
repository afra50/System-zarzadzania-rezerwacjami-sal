using System.Windows;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno do zarządzania rezerwacjami.
    /// </summary>
    public partial class ManageReservationsWindow : Window
    {
        /// <summary>
        /// Inicjalizacja okna zarządzania rezerwacjami.
        /// </summary>
        public ManageReservationsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Przeglądaj rezerwacje" i otwiera okno z listą rezerwacji.
        /// </summary>
        private void BrowseReservations_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno z rezerwacjami
            ReservationsListWindow reservationsListWindow = new ReservationsListWindow();
            reservationsListWindow.Show();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Otwórz statystyki" i otwiera okno ze statystykami.
        /// </summary>
        private void OpenStatisticsWindow_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno ze statystykami
            StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno wyświetlające rezerwacje użytkownika.
    /// </summary>
    public partial class MyReservationsWindow : Window
    {
        private readonly ReservationService _reservationService;

        /// <summary>
        /// Inicjalizacja okna rezerwacji użytkownika.
        /// </summary>
        public MyReservationsWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            LoadUserReservations();
        }

        /// <summary>
        /// Ładowanie rezerwacji użytkownika.
        /// Filtruje rezerwacje, usuwając anulowane i zakończone wczoraj.
        /// </summary>
        private async void LoadUserReservations()
        {
            var reservations = await _reservationService.GetUserReservationsAsync(SessionManager.UserEmail);
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            var activeReservations = reservations
                .Where(r => r.Status != "cancelled" && r.EndDateTime > yesterday)
                .ToList();

            ReservationsListView.ItemsSource = activeReservations;
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Anuluj rezerwację".
        /// </summary>
        private async void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsListView.SelectedItem is Reservation selectedReservation)
            {
                var confirmation = MessageBox.Show("Czy na pewno chcesz anulować tę rezerwację?", "Potwierdzenie", MessageBoxButton.YesNo);
                if (confirmation == MessageBoxResult.Yes)
                {
                    bool success = await _reservationService.CancelReservationAsync(selectedReservation.IdReservation);
                    if (success)
                    {
                        MessageBox.Show("Rezerwacja została anulowana!");
                        LoadUserReservations(); // Odświeżenie listy rezerwacji
                    }
                    else
                    {
                        MessageBox.Show("Błąd podczas anulowania rezerwacji.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać rezerwację do anulowania.");
            }
        }
    }
}

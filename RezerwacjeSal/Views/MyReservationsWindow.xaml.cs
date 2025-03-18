using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class MyReservationsWindow : Window
    {
        private readonly ReservationService _reservationService;

        public MyReservationsWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            LoadUserReservations();
        }

        private async void LoadUserReservations()
        {
            // Pobierz rezerwacje użytkownika z sesji
            var reservations = await _reservationService.GetUserReservationsAsync(SessionManager.UserEmail);

            // Filtrujemy rezerwacje, usuwając te o statusie 'cancelled'
            var activeReservations = reservations.Where(r => r.Status != "cancelled").ToList();

            // Ustawiamy przefiltrowaną listę rezerwacji do ListView
            ReservationsListView.ItemsSource = activeReservations;
        }


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
                        LoadUserReservations(); // Odśwież listę rezerwacji
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

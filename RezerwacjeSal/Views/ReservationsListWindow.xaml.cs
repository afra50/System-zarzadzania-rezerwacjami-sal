using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class ReservationsListWindow : Window
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;

        public ReservationsListWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            _roomService = new RoomService();
            LoadRooms(); // Załaduj pokoje do ComboBox
            LoadReservations(); // Załaduj rezerwacje
        }

        // Ładowanie sal do ComboBox
        private async void LoadRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            RoomComboBox.ItemsSource = rooms;
            RoomComboBox.DisplayMemberPath = "Name";
            RoomComboBox.SelectedValuePath = "Id";
        }

        // Ładowanie rezerwacji z serwera
        private async void LoadReservations(int? roomId = null)
        {
            // Pobierz rezerwacje z serwera, z możliwością filtrowania po sali
            var reservations = await _reservationService.GetReservationsAsync(roomId);
            ReservationsListView.ItemsSource = reservations;
        }

        // Obsługuje zmianę sali z ComboBox
        private void RoomComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is Room selectedRoom)
            {
                // Filtrowanie rezerwacji po sali
                LoadReservations(selectedRoom.Id);
            }
            else
            {
                // Załaduj wszystkie rezerwacje, jeśli sala nie została wybrana
                LoadReservations();
            }
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
                        LoadReservations(); // Odśwież listę rezerwacji
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

        private async void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsListView.SelectedItem is Reservation selectedReservation)
            {
                var confirmation = MessageBox.Show("Czy na pewno chcesz potwierdzić tę rezerwację?", "Potwierdzenie", MessageBoxButton.YesNo);
                if (confirmation == MessageBoxResult.Yes)
                {
                    bool success = await _reservationService.ConfirmReservationAsync(selectedReservation.IdReservation);
                    if (success)
                    {
                        MessageBox.Show("Rezerwacja została potwierdzona!");
                        LoadReservations(); // Odśwież listę rezerwacji
                    }
                    else
                    {
                        MessageBox.Show("Błąd podczas potwierdzania rezerwacji.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać rezerwację do potwierdzenia.");
            }
        }

    }
}

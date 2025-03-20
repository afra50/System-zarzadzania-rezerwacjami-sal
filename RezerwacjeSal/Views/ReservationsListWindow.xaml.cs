using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno wyświetlające listę rezerwacji.
    /// </summary>
    public partial class ReservationsListWindow : Window
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;

        /// <summary>
        /// Inicjalizacja okna z listą rezerwacji.
        /// </summary>
        public ReservationsListWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            _roomService = new RoomService();
            LoadRooms(); // Załaduj pokoje do ComboBox
            LoadReservations(); // Załaduj rezerwacje
        }

        /// <summary>
        /// Ładowanie dostępnych sal do ComboBox.
        /// </summary>
        private async void LoadRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            RoomComboBox.ItemsSource = rooms;
            RoomComboBox.DisplayMemberPath = "Name";
            RoomComboBox.SelectedValuePath = "Id";
        }

        /// <summary>
        /// Ładowanie rezerwacji z serwera z możliwością filtrowania po sali.
        /// </summary>
        private async void LoadReservations(int? roomId = null)
        {
            var reservations = await _reservationService.GetReservationsAsync(roomId);
            ReservationsListView.ItemsSource = reservations;
        }

        /// <summary>
        /// Obsługuje zmianę sali z ComboBox i filtruje rezerwacje.
        /// </summary>
        private void RoomComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is Room selectedRoom)
            {
                LoadReservations(selectedRoom.Id); // Filtrowanie po sali
            }
            else
            {
                LoadReservations(); // Załaduj wszystkie rezerwacje
            }
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
                        LoadReservations(); // Odświeżenie listy rezerwacji
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

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Potwierdź rezerwację".
        /// </summary>
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
                        LoadReservations(); // Odświeżenie listy rezerwacji
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

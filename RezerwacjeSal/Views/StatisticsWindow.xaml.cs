using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Services;
using RezerwacjeSal.Models;
using System.Data;
using System.Diagnostics;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno wyświetlające statystyki rezerwacji sal.
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;

        /// <summary>
        /// Inicjalizuje okno z danymi statystycznymi, wczytując statystyki najczęściej rezerwowanych sal.
        /// </summary>
        public StatisticsWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            _roomService = new RoomService();
            LoadStatistics();
        }

        /// <summary>
        /// Wczytuje dane statystyczne: najczęściej rezerwowane sale, rezerwacje wg dni tygodnia i miesięcy.
        /// Dane są sortowane malejąco według liczby rezerwacji.
        /// </summary>
        private async void LoadStatistics()
        {
            try
            {
                // Najczęściej wybierane sale (bez zmian)
                var mostBookedRooms = await _roomService.GetMostBookedRoomsAsync();
                MostBookedRoomsListView.ItemsSource = mostBookedRooms;

                // Rezerwacje wg dni tygodnia (malejąco)
                var reservationsByDay = await _reservationService.GetReservationsByDayAsync();
                reservationsByDay = reservationsByDay
                    .OrderByDescending(d => d.ReservationCount)
                    .ToList();
                ReservationsByDayListView.ItemsSource = reservationsByDay;

                // Rezerwacje wg miesięcy (malejąco)
                var reservationsByMonth = await _reservationService.GetReservationsByMonthAsync();
                reservationsByMonth = reservationsByMonth
                    .OrderByDescending(m => m.ReservationCount)
                    .ToList();
                ReservationsByMonthListView.ItemsSource = reservationsByMonth;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania statystyk: {ex.Message}");
            }
        }

    }
}

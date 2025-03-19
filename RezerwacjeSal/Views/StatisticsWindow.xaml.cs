using System;
using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Services;
using RezerwacjeSal.Models;
using System.Data;
using System.Diagnostics;

namespace RezerwacjeSal.Views
{
    public partial class StatisticsWindow : Window
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;

        public StatisticsWindow()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            _roomService = new RoomService();
            LoadStatistics();
        }

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

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
                // Załaduj najczęściej wybierane sale
                var mostBookedRooms = await _roomService.GetMostBookedRoomsAsync();
                MostBookedRoomsListView.ItemsSource = mostBookedRooms;

                // Załaduj rezerwacje wg dni tygodnia
                var reservationsByDay = await _reservationService.GetReservationsByDayAsync();
                ReservationsByDayListView.ItemsSource = reservationsByDay;

                // Załaduj rezerwacje wg miesięcy
                var reservationsByMonth = await _reservationService.GetReservationsByMonthAsync();
                ReservationsByMonthListView.ItemsSource = reservationsByMonth;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania statystyk: {ex.Message}");
            }
        }

    }
}

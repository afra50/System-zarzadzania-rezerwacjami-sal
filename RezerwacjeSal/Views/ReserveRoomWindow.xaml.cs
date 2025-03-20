using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Web;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno odpowiedzialne za rezerwację sali.
    /// </summary>
    public partial class ReserveRoomWindow : Window
    {
        private readonly RoomService _roomService;
        private readonly ReservationService _reservationService;
        private readonly string _googleMapsApiKey;
        private List<Room> _rooms = new();

        /// <summary>
        /// Inicjalizuje okno rezerwacji sali oraz klucz API Google Maps.
        /// </summary>
        public ReserveRoomWindow()
        {
            InitializeComponent();
            _roomService = new RoomService();
            _reservationService = new ReservationService();
            _googleMapsApiKey = System.Configuration.ConfigurationManager.AppSettings["GoogleMapsApiKey"]
                ?? throw new InvalidOperationException("Brak klucza API Google Maps!");

            MapView.Loaded += async (s, e) =>
            {
                await MapView.EnsureCoreWebView2Async();
            };

            LoadRooms();
        }

        /// <summary>
        /// Ładowanie dostępnych sal do ComboBox.
        /// </summary>
        private async void LoadRooms()
        {
            _rooms = await _roomService.GetRoomsAsync();
            RoomComboBox.ItemsSource = _rooms;
            RoomComboBox.DisplayMemberPath = "Name";
            RoomComboBox.SelectedValuePath = "Id";
        }

        /// <summary>
        /// Obsługuje zmianę wybranej sali w ComboBoxie, wczytuje mapę i dostępne daty.
        /// </summary>
        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is Room selectedRoom)
            {
                LoadGoogleMap(selectedRoom.Latitude, selectedRoom.Longitude);
                LoadAvailableDates(selectedRoom.Id);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku rezerwacji sali, waliduje dane wejściowe i zapisuje rezerwację.
        /// </summary>
        private async void ReserveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is not Room selectedRoom)
            {
                MessageBox.Show("Wybierz salę!");
                return;
            }

            if (!DateTime.TryParse($"{StartDatePicker.Text} {StartTimeBox.Text}", out DateTime startDateTime) ||
                !DateTime.TryParse($"{EndDatePicker.Text} {EndTimeBox.Text}", out DateTime endDateTime))
            {
                MessageBox.Show("Podaj poprawne daty i godziny!");
                return;
            }

            if (startDateTime < DateTime.Now)
            {
                MessageBox.Show("Data rozpoczęcia musi być w przyszłości!");
                return;
            }

            if (endDateTime <= startDateTime)
            {
                MessageBox.Show("Data zakończenia musi być późniejsza niż rozpoczęcia!");
                return;
            }

            var reservation = new Reservation
            {
                RoomId = selectedRoom.Id,
                UserEmail = SessionManager.UserEmail ?? "test@example.com",
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };

            bool success = await _reservationService.CreateReservationAsync(reservation);

            if (success)
            {
                MessageBox.Show("Rezerwacja została zapisana!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Błąd podczas zapisu rezerwacji!");
            }
        }

        /// <summary>
        /// Zamknięcie okna rezerwacji bez zapisywania zmian.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private Dictionary<DateTime, List<OccupiedTime>> occupiedTimeSlots = new();

        /// <summary>
        /// Ładowanie dostępnych dat na podstawie zarezerwowanych godzin w danej sali.
        /// </summary>
        private async void LoadAvailableDates(int roomId)
        {
            var reservations = await _reservationService.GetOccupiedTimesAsync(roomId);

            Dictionary<DateTime, List<(DateTime Start, DateTime End)>> occupiedHours = new();
            occupiedTimeSlots.Clear();

            foreach (var res in reservations)
            {
                DateTime start = res.StartDateTimeLocal;
                DateTime end = res.EndDateTimeLocal;
                DateTime dateOnly = start.Date;

                if (!occupiedHours.ContainsKey(dateOnly))
                {
                    occupiedHours[dateOnly] = new List<(DateTime, DateTime)>();
                }

                occupiedHours[dateOnly].Add((start, end));

                if (!occupiedTimeSlots.ContainsKey(dateOnly))
                {
                    occupiedTimeSlots[dateOnly] = new List<OccupiedTime>();
                }

                occupiedTimeSlots[dateOnly].Add(new OccupiedTime
                {
                    StartTime = start.ToString("HH:mm"),
                    EndTime = end.ToString("HH:mm")
                });
            }

            AvailabilityCalendar.BlackoutDates.Clear();
            AvailabilityCalendar.DisplayDateStart = DateTime.Today;
            AvailabilityCalendar.DisplayDateEnd = DateTime.Today.AddMonths(3);

            foreach (var entry in occupiedHours)
            {
                DateTime day = entry.Key;
                var times = entry.Value;

                // 🟢 Sprawdzamy, czy wszystkie godziny od 00:00 do 23:59 są zajęte
                bool fullDayOccupied = true;
                for (int hour = 0; hour < 24; hour++)
                {
                    DateTime checkTime = new DateTime(day.Year, day.Month, day.Day, hour, 0, 0);
                    if (!times.Any(t => t.Start <= checkTime && t.End > checkTime))
                    {
                        fullDayOccupied = false;
                        break;
                    }
                }

                if (fullDayOccupied)
                {
                    AvailabilityCalendar.BlackoutDates.Add(new CalendarDateRange(day));
                }
            }
        }


        /// <summary>
        /// Obsługuje zmianę wybranej daty w kalendarzu, wyświetlając dostępne godziny.
        /// </summary>
        private void AvailabilityCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AvailabilityCalendar.SelectedDate is DateTime selectedDate)
            {

                if (occupiedTimeSlots.ContainsKey(selectedDate))
                {
                    OccupiedTimesListView.ItemsSource = occupiedTimeSlots[selectedDate];
                }
                else
                {
                    OccupiedTimesListView.ItemsSource = new List<OccupiedTime>();
                }
            }
        }

        /// <summary>
        /// Ładowanie mapy Google z lokalizacją sali.
        /// </summary>
        private void LoadGoogleMap(decimal latitude, decimal longitude)
        {
            string html = $@"
                <html>
                <head>
                    <script src='https://maps.googleapis.com/maps/api/js?key={_googleMapsApiKey}&libraries=places'></script>
                    <script>
                        function initMap() {{
                            let lat = {latitude.ToString().Replace(",", ".")};
                            let lng = {longitude.ToString().Replace(",", ".")};

                            let map = new google.maps.Map(document.getElementById('map'), {{
                                center: {{ lat: lat, lng: lng }},
                                zoom: 15
                            }});

                            let marker = new google.maps.Marker({{
                                position: {{ lat: lat, lng: lng }},
                                map: map
                            }});
                        }}
                    </script>
                </head>
                <body onload='initMap()'>
                    <div id='map' style='width:100%; height:100%;'></div>
                </body>
                </html>";

            MapView.NavigateToString(html);
        }

        /// <summary>
        /// Otwiera trasę do sali w Google Maps w przeglądarce.
        /// </summary>
        private void OpenRouteInGoogleMaps(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is not Room selectedRoom)
            {
                MessageBox.Show("Najpierw wybierz salę!");
                return;
            }

            string googleMapsUrl = $"https://www.google.com/maps/dir/?api=1&destination={selectedRoom.Latitude.ToString().Replace(",", ".")},{selectedRoom.Longitude.ToString().Replace(",", ".")}";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = googleMapsUrl,
                UseShellExecute = true
            });
        }


        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Name == "StartTimeBox")
                    StartTimePlaceholder.Visibility = Visibility.Collapsed;

                if (textBox.Name == "EndTimeBox")
                    EndTimePlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "StartTimeBox")
                    StartTimePlaceholder.Visibility = Visibility.Visible;

                if (textBox.Name == "EndTimeBox")
                    EndTimePlaceholder.Visibility = Visibility.Visible;
            }
        }

    }
}

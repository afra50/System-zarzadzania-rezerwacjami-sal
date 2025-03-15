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

namespace RezerwacjeSal.Views
{
    public partial class ReserveRoomWindow : Window
    {
        private readonly RoomService _roomService;
        private readonly ReservationService _reservationService;
        private readonly string _googleMapsApiKey;
        private List<Room> _rooms = new();

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


        private async void LoadRooms()
        {
            _rooms = await _roomService.GetRoomsAsync();
            RoomComboBox.ItemsSource = _rooms;
            RoomComboBox.DisplayMemberPath = "Name";
            RoomComboBox.SelectedValuePath = "Id";
        }

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomComboBox.SelectedItem is Room selectedRoom)
            {
                LoadGoogleMap(selectedRoom.Latitude, selectedRoom.Longitude);
            }
        }

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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

      
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

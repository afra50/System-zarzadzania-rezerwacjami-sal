using System;
using System.Configuration;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using RezerwacjeSal.Models;
using System.Text.Json;
using Newtonsoft.Json;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno edytowania sali. Pozwala na edycję informacji o sali, takich jak nazwa, adres, lokalizacja i liczba miejsc.
    /// </summary>
    public partial class EditRoomWindow : Window
    {
        /// <summary>
        /// Obiekt sali, który będzie edytowany.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Klucz API Google Maps dla załadowania mapy.
        /// </summary>
        private readonly string googleMapsApiKey;

        /// <summary>
        /// Konstruktor okna, w którym wypełniane są dane o sali do edycji.
        /// </summary>
        /// <param name="room">Obiekt sali, który ma zostać edytowany.</param>
        public EditRoomWindow(Room room)
        {
            InitializeComponent();
            Room = room ?? new Room();

            // Pobranie klucza z App.config
            googleMapsApiKey = ConfigurationManager.AppSettings["GoogleMapsApiKey"] ?? throw new InvalidOperationException("Brak klucza API Google Maps!");

            // Wypełnianie pól
            RoomNameBox.Text = Room.Name;
            RoomAddressBox.Text = Room.Address;
            RoomSeatsBox.Text = Room.Seats > 0 ? Room.Seats.ToString() : "";
            RoomDescriptionBox.Text = Room.Description;
            RoomLatitudeBox.Text = Room.Latitude.ToString();
            RoomLongitudeBox.Text = Room.Longitude.ToString();

            // Załaduj mapę po załadowaniu WebView2
            MapView.Loaded += async (s, e) =>
            {
                await MapView.EnsureCoreWebView2Async();
                MapView.WebMessageReceived += MapView_WebMessageReceived;
                LoadGoogleMap();
            };
        }

        /// <summary>
        /// Ładuje mapę Google z zaznaczoną lokalizacją sali.
        /// </summary>
        private void LoadGoogleMap()
        {
            string html = $@"
        <html>
        <head>
            <script src='https://maps.googleapis.com/maps/api/js?key={googleMapsApiKey}&libraries=places'></script>
            <script>
                let map, marker, geocoder;

                function initMap() {{
                    let lat = parseFloat({Room.Latitude.ToString().Replace(",", ".")});
                    let lng = parseFloat({Room.Longitude.ToString().Replace(",", ".")});

                    map = new google.maps.Map(document.getElementById('map'), {{
                        center: {{ lat: lat, lng: lng }},
                        zoom: 15
                    }});

                    marker = new google.maps.Marker({{
                        position: {{ lat: lat, lng: lng }},
                        map: map,
                        draggable: true
                    }});

                    geocoder = new google.maps.Geocoder();

                    // Obsługa przesuwania pinezki
                    google.maps.event.addListener(marker, 'dragend', function() {{
                        let newLat = marker.getPosition().lat();
                        let newLng = marker.getPosition().lng();

                        window.chrome.webview.postMessage(JSON.stringify({{ lat: newLat, lng: newLng }}));

                        // Pobieranie adresu po przesunięciu
                        geocoder.geocode({{ location: marker.getPosition() }}, function(results, status) {{
                            if (status === 'OK' && results[0]) {{
                                window.chrome.webview.postMessage(JSON.stringify({{ address: results[0].formatted_address }}));
                            }}
                        }});
                    }});
                }}
            </script>
        </head>
        <body onload='initMap()'>
            <div id='map' style='width:100%; height:100%;'></div>
        </body>
        </html>
    ";

            MapView.NavigateToString(html);
        }

        /// <summary>
        /// Odbiera wiadomości z mapy i aktualizuje dane o lokalizacji i adresie.
        /// </summary>
        private void MapView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                string jsonData = e.WebMessageAsJson;
                jsonData = jsonData.Trim('"').Replace("\\", "");

                var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);

                if (data != null)
                {
                    if (data.ContainsKey("lat") && data.ContainsKey("lng"))
                    {
                        RoomLatitudeBox.Text = data["lat"].ToString();
                        RoomLongitudeBox.Text = data["lng"].ToString();
                    }
                    else if (data.ContainsKey("address"))
                    {
                        RoomAddressBox.Text = data["address"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd mapy: {ex.Message}");
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zapisz".
        /// Zapisuje edytowane dane sali do bazy danych.
        /// </summary>
        private async void SaveRoom_Click(object sender, RoutedEventArgs e)
        {
            Room.Name = RoomNameBox.Text.Trim();
            Room.Address = RoomAddressBox.Text.Trim();
            Room.Seats = int.TryParse(RoomSeatsBox.Text, out int seats) ? seats : 0;
            Room.Description = RoomDescriptionBox.Text.Trim();

            // **🔹 Konwersja na liczby i przypisanie do obiektu Room**
            if (decimal.TryParse(RoomLatitudeBox.Text.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal lat))
            {
                Room.Latitude = lat;
            }
            else
            {
                MessageBox.Show("Błąd konwersji Latitude!");
            }

            if (decimal.TryParse(RoomLongitudeBox.Text.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal lng))
            {
                Room.Longitude = lng;
            }
            else
            {
                MessageBox.Show("Błąd konwersji Longitude!");
            }

            // **🔹 Sprawdzenie, czy nazwa sali nie jest pusta**
            if (string.IsNullOrWhiteSpace(Room.Name))
            {
                MessageBox.Show("Błąd: Nazwa sali nie może być pusta!");
                return;
            }

            // **🔹 Wysłanie danych do API**
            RoomService roomService = new RoomService();
            bool success = await roomService.UpdateRoomAsync(Room);

            if (success)
            {
                MessageBox.Show("Zmiany zostały zapisane!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Błąd zapisu danych.");
            }
        }


        /// <summary>
        /// Obsługuje kliknięcie przycisku "Anuluj".
        /// Anuluje edycję i zamyka okno.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

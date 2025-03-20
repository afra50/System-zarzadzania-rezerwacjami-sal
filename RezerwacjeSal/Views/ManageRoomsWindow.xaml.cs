using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno do zarządzania salami.
    /// </summary>
    public partial class ManageRoomsWindow : Window
    {
        public List<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Inicjalizacja okna zarządzania salami i załadowanie dostępnych sal.
        /// </summary>
        public ManageRoomsWindow()
        {
            InitializeComponent();
            LoadRooms();
        }

        /// <summary>
        /// Ładuje dostępne sale z serwera poprzez API.
        /// </summary>
        private async void LoadRooms()
        {
            // Pobieranie danych z API
            RoomService roomService = new RoomService();
            Rooms = await roomService.GetRoomsAsync();

            if (Rooms.Count == 0)
            {
                MessageBox.Show("Brak dostępnych sal.");
            }

            RoomsListView.ItemsSource = Rooms;
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Edytuj salę" i otwiera okno edycji sali.
        /// </summary>
        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsListView.SelectedItem is Room selectedRoom)
            {
                EditRoomWindow editRoomWindow = new EditRoomWindow(selectedRoom);
                editRoomWindow.ShowDialog();
                LoadRooms(); // Odświeżenie listy po edycji
            }
            else
            {
                MessageBox.Show("Wybierz salę do edycji.");
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zamknij" i zamyka okno.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


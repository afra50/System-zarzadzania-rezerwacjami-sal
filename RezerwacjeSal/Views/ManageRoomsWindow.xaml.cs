using System.Collections.Generic;
using System.Windows;
using RezerwacjeSal.Models;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class ManageRoomsWindow : Window
    {
        public List<Room> Rooms { get; set; } = new List<Room>();

        public ManageRoomsWindow()
        {
            InitializeComponent();
            LoadRooms();
        }

        private async void LoadRooms()
        {
            // Pobieranie danych z API
            Rooms = await RoomService.GetRoomsAsync();

            if (Rooms.Count == 0)
            {
                MessageBox.Show("Brak dostępnych sal.");
            }

            RoomsListView.ItemsSource = Rooms;
        }

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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

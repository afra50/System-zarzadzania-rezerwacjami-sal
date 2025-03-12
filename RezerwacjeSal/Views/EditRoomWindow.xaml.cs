using System;
using System.Windows;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Views
{
    public partial class EditRoomWindow : Window
    {
        public Room Room { get; set; }

        public EditRoomWindow(Room room)
        {
            InitializeComponent();
            Room = room ?? new Room(); // Jeśli null, utwórz nowy obiekt, żeby uniknąć błędów

            // Wypełnienie pól danymi
            RoomNameBox.Text = Room.Name;
            RoomAddressBox.Text = Room.Address;
            RoomSeatsBox.Text = Room.Seats > 0 ? Room.Seats.ToString() : "";
            RoomDescriptionBox.Text = Room.Description;
        }

        private void SaveRoom_Click(object sender, RoutedEventArgs e)
        {
            // Pobranie wartości z pól
            Room.Name = RoomNameBox.Text;
            Room.Address = RoomAddressBox.Text;
            Room.Seats = int.TryParse(RoomSeatsBox.Text, out int seats) ? seats : 0;
            Room.Description = RoomDescriptionBox.Text;

            // Można dodać zapis do bazy danych tutaj...

            MessageBox.Show("Zmiany zapisane!");
            this.DialogResult = true; // Zamknięcie okna z wynikiem sukcesu
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Zamknięcie okna bez zapisywania zmian
            this.Close();
        }
    }
}

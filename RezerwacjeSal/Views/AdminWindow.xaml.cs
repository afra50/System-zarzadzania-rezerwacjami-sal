using System.Windows;

namespace RezerwacjeSal.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Brak dostępu!");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void ManageRooms_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomsWindow manageRoomsWindow = new ManageRoomsWindow();
            manageRoomsWindow.Show();
        }

        private void ManageReservations_Click(object sender, RoutedEventArgs e)
        {
            ManageReservationsWindow manageReservationsWindow = new ManageReservationsWindow();
            manageReservationsWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.UserName = null;
            SessionManager.UserEmail = null;
            SessionManager.UserRole = null;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

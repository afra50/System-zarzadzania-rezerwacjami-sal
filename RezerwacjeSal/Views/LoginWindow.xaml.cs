using System;
using System.Windows;
using System.Windows.Controls;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new AuthService();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false; // Blokowanie przycisku, żeby nie klikać kilka razy

            try
            {
                string email = EmailBox.Text == "Email" ? "" : EmailBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Podaj e-mail i hasło!");
                    return;
                }

                bool success = await _apiService.LoginUser(email, password);

                if (success)
                {
                    MessageBox.Show("Logowanie zakończone sukcesem!");

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie udało się zalogować. Sprawdź dane.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd aplikacji: {ex.Message}");
            }
            finally
            {
                LoginButton.IsEnabled = true; // Włączenie przycisku z powrotem
            }
        }

        // Obsługa placeholdera dla TextBox (Email)
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Email")
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Email";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Obsługa placeholdera dla PasswordBox
        private void RemovePasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null && passwordBox.Tag?.ToString() == "Placeholder")
            {
                passwordBox.Clear();
                passwordBox.Tag = null;
                passwordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Tag = "Placeholder";
                passwordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _apiService;

        public RegisterWindow()
        {
            InitializeComponent();
            _apiService = new AuthService();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterButton.IsEnabled = false; // Blokowanie przycisku, żeby nie klikać kilka razy

            try
            {
                string name = NameBox.Text == "Imię i nazwisko" ? "" : NameBox.Text;
                string email = EmailBox.Text == "Email" ? "" : EmailBox.Text;
                string password = PasswordBox.Password;
                string role = "client"; // Automatyczne ustawienie roli

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Wszystkie pola są wymagane!");
                    return;
                }

                bool success = await _apiService.RegisterUser(name, email, password, role);

                if (success)
                {
                    MessageBox.Show("Rejestracja zakończona sukcesem!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie udało się zarejestrować. Sprawdź dane.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd aplikacji: {ex.Message}");
            }
            finally
            {
                RegisterButton.IsEnabled = true; // Włączenie przycisku z powrotem
            }
        }

        // Obsługa usuwania i dodawania placeholdera
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && (textBox.Text == "Imię i nazwisko" || textBox.Text == "Email"))
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "NameBox")
                    textBox.Text = "Imię i nazwisko";
                else if (textBox.Name == "EmailBox")
                    textBox.Text = "Email";

                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}

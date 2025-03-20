using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using RezerwacjeSal.Services;

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno rejestracji nowego użytkownika.
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _apiService;

        /// <summary>
        /// Inicjalizacja okna rejestracji.
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();
            _apiService = new AuthService();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarejestruj się".
        /// Sprawdza poprawność danych i wykonuje rejestrację użytkownika.
        /// </summary>
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterButton.IsEnabled = false; // Blokowanie przycisku, żeby nie klikać kilka razy

            try
            {
                string name = NameBox.Text == "Imię i nazwisko" ? "" : NameBox.Text.Trim();
                string email = EmailBox.Text == "Email" ? "" : EmailBox.Text.Trim();
                string password = PasswordBox.Password;
                string role = "client"; // Automatyczne ustawienie roli

                // **🔹 Sprawdzenie, czy pola nie są puste**
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Wszystkie pola są wymagane!");
                    return;
                }

                // **🔹 Walidacja e-maila**
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Błąd: Podano niepoprawny adres e-mail!");
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

        /// <summary>
        /// Metoda sprawdzająca poprawność e-maila.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        /// <summary>
        /// Usuwanie placeholdera w polu tekstowym.
        /// </summary>
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && (textBox.Text == "Imię i nazwisko" || textBox.Text == "Email"))
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        /// <summary>
        /// Dodawanie placeholdera w polu tekstowym, jeśli jest puste.
        /// </summary>
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

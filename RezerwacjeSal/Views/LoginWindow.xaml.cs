using System;
using System.Windows;
using System.Windows.Controls;
using RezerwacjeSal.Services;
using RezerwacjeSal.Models; // <-- Upewnij się, że jest to dodane

namespace RezerwacjeSal.Views
{
    /// <summary>
    /// Okno logowania dla użytkowników.
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AuthService _apiService;

        /// <summary>
        /// Inicjalizacja okna logowania.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new AuthService();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku logowania.
        /// Przeprowadza logowanie użytkownika i otwiera odpowiednie okno w zależności od roli.
        /// </summary>
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;

            try
            {
                string email = EmailBox.Text == "Email" ? "" : EmailBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Podaj e-mail i hasło!");
                    LoginButton.IsEnabled = true;
                    return;
                }

                // Teraz user to obiekt UserDto? a nie bool
                UserDto? user = await _apiService.LoginUser(email, password);

                if (user != null)
                {
                    // user ma wlasciwosci Name, Email, Role
                    if (!string.IsNullOrWhiteSpace(user.Name) &&
                        !string.IsNullOrWhiteSpace(user.Email) &&
                        !string.IsNullOrWhiteSpace(user.Role))
                    {
                        // Zapisanie użytkownika w sesji (opcjonalnie)
                        SessionManager.UserName = user.Name;
                        SessionManager.UserEmail = user.Email;
                        SessionManager.UserRole = user.Role;

                        MessageBox.Show($"Zalogowano jako {user.Name} ({user.Role})");

                        // Otwieranie odpowiedniego okna
                        if (SessionManager.IsAdmin)
                        {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                        }
                        else
                        {
                            ClientWindow clientWindow = new ClientWindow();
                            clientWindow.Show();
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się zalogować. Brak danych użytkownika.");
                    }
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
                LoginButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Obsługuje usuwanie placeholdera w polu e-mail.
        /// </summary>
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Email")
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        /// <summary>
        /// Dodaje placeholdera w polu e-mail, jeśli jest pusty.
        /// </summary>
        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Email";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        /// <summary>
        /// Obsługuje usuwanie placeholdera w polu hasła.
        /// </summary>
        private void RemovePasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            PasswordBox? passwordBox = sender as PasswordBox;
            if (passwordBox != null && passwordBox.Tag?.ToString() == "Placeholder")
            {
                passwordBox.Clear();
                passwordBox.Tag = null;
                passwordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        /// <summary>
        /// Dodaje placeholdera w polu hasła, jeśli jest puste.
        /// </summary>
        private void AddPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            PasswordBox? passwordBox = sender as PasswordBox;
            if (passwordBox != null && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Tag = "Placeholder";
                passwordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}

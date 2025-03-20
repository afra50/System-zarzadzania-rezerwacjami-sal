using System.Windows;
using RezerwacjeSal.Views;

namespace RezerwacjeSal
{
    /// <summary>
    /// Klasa odpowiedzialna za uruchomienie aplikacji i wy�wietlenie g��wnego okna.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Metoda wywo�ywana przy starcie aplikacji, tworzy instancj� g��wnego okna i je wy�wietla.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Uruchamia domy�ln� logik� startow�
            MainWindow mainWindow = new MainWindow(); // Tworzenie instancji g��wnego okna
            mainWindow.Show(); // Wy�wietlanie g��wnego okna aplikacji
        }
    }
}

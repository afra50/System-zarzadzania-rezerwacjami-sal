using System.Windows;
using RezerwacjeSal.Views;

namespace RezerwacjeSal
{
    /// <summary>
    /// Klasa odpowiedzialna za uruchomienie aplikacji i wyœwietlenie g³ównego okna.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Metoda wywo³ywana przy starcie aplikacji, tworzy instancjê g³ównego okna i je wyœwietla.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Uruchamia domyœln¹ logikê startow¹
            MainWindow mainWindow = new MainWindow(); // Tworzenie instancji g³ównego okna
            mainWindow.Show(); // Wyœwietlanie g³ównego okna aplikacji
        }
    }
}

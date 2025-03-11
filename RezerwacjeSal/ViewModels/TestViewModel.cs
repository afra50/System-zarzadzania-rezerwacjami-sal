using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class TestViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _testMessage = "Sprawdzanie po³¹czenia..."; // Domyœlnie czeka na wynik

    public string TestMessage
    {
        get => _testMessage;
        set { _testMessage = value; OnPropertyChanged(); }
    }

    public TestViewModel()
    {
        _apiService = new ApiService();
        LoadTestMessage();
    }

    private async void LoadTestMessage()
    {
        bool apiIsUp = await _apiService.IsApiAvailable();
        TestMessage = apiIsUp ? "API dzia³a!" : "Brak dostêpu do API!";
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

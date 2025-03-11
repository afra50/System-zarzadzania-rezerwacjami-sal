using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class TestViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _testMessage = "£adowanie..."; // Ustaw domyœln¹ wartoœæ

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
        TestMessage = await _apiService.GetTestMessage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

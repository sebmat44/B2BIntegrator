using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using B2BIntegrator.App.Services;

namespace B2BIntegrator.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NbpApiService _nbpService = new();

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "Gotowe. Kliknij przycisk," +
                                                        " aby pobrać kurs dolara.";

    [RelayCommand]
    private async Task FetchUsdRateAsync()
    {
        StatusMessage = "Pobieram dane z NBP API...";

        decimal? rate = await _nbpService.GetUsdExchangeRateAsync();

        if (rate.HasValue)
        {
            StatusMessage = $"Aktualny kurs dolara: {rate.Value} PLN";
        }
        else
        {
            StatusMessage = "Błąd: nie można było pobrać kursu.";
        }
    }
}


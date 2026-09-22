using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using B2BIntegrator.App.Services;
using System.Threading.Tasks;

namespace B2BIntegrator.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NbpApiService _nbpService = new();
    private readonly ViesApiService _viesService = new();

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "Gotowe. Kliknij przycisk," +
                                                        " aby pobrać kurs dolara.";
    [ObservableProperty]
    public partial string CountryCode { get; set; } = "PL";
    [ObservableProperty]
    public partial string VatNumber { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string ViesResult { get; set; } = string.Empty;

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

    [RelayCommand]
    private async Task VerifyVatNumberAsync()
    {
        if (string.IsNullOrWhiteSpace(VatNumber))
        {
            ViesResult = "Proszę wprowadzić numer NIP.";
            return;
        }

        ViesResult = "Łączenie z europejską bazą VIES...";

        var (isValid, companyName, address) = await _viesService.VerifyVatAsync(CountryCode, VatNumber);

        if (isValid)
        {
            ViesResult = $"NIP POPRAWNY\nFirma: {companyName}\nAdres: {address}";
        }
        else
        {
            ViesResult = "NIP NIEPOPRAWNY lub błąd połączenia.";
        }
    }
}


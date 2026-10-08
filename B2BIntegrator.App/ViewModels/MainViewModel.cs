using B2BIntegrator.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace B2BIntegrator.App.ViewModels;

public partial class MainViewModel(INbpApiService nbpService, 
    IViesApiService viesService,
    ILogger<MainViewModel> logger) : ObservableObject
{
    private readonly INbpApiService _nbpService = nbpService;
    private readonly IViesApiService _viesService = viesService;
    private readonly ILogger<MainViewModel> _logger = logger;

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "Gotowe. Kliknij przycisk," +
                                                        " aby pobrać kurs dolara.";

    [ObservableProperty]
    public partial string CountryCode { get; set; } = "PL";
    [ObservableProperty]
    public partial string VatNumber { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string ViesResult { get; set; } = string.Empty;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task FetchUsdRateAsync(CancellationToken token)
    {
        StatusMessage = "Pobieram dane z NBP API...";

        _logger.LogInformation("Rozpoczęto pobieranie kursu USD.");

        try
        {
            //showcase purposes, can be cancelled using token
            await Task.Delay(3000, token);

            decimal? rate = await _nbpService.GetUsdExchangeRateAsync(token);

            if (rate.HasValue)
            {
                StatusMessage = $"Aktualny kurs dolara: " +
                    $"{rate.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)} PLN";
                _logger.LogInformation("Pomyślnie pobrano kurs USD: {Rate}", rate.Value);
            }
            else
            {
                StatusMessage = "Błąd: nie można było pobrać kursu.";
                _logger.LogWarning("API NBP zwróciło pusty wynik (null).");
            }
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Akcja została anulowana przez użytkownika.";
            _logger.LogInformation("Użytkownik anulował pobieranie kursu USD.");

        }
        catch (Exception ex)
        {
            StatusMessage = "Wystąpił nieoczekiwany błąd. Sprawdź plik logów.";

            _logger.LogError(ex, "Wystąpił krytyczny błąd podczas pobierania kursu USD.");
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


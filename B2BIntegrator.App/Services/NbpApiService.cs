using System.Net.Http;
using System.Net.Http.Json;
using B2BIntegrator.App.Models;

namespace B2BIntegrator.App.Services;

public class NbpApiService(HttpClient httpClient) : INbpApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<decimal?> GetUsdExchangeRateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            //rest of the url address is taken from configured httpClient
            var response = 
                await _httpClient.GetFromJsonAsync<NbpResponse>(
                    "exchangerates/rates/A/USD/", cancellationToken);

            return response?.Rates?[0].Mid;
        }
        catch (TaskCanceledException)
        {
            //exception goes to ViewModel
            throw;
        }
        catch (Exception ex)
        {
            //TO DO dodac logowanie
            return null;
        }
    }
}
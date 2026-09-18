using System.Net.Http;
using System.Net.Http.Json;
using B2BIntegrator.App.Models;

namespace B2BIntegrator.App.Services;

public class NbpApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<decimal?> GetUsdExchangeRateAsync()
    {
        string url = "http://api.nbp.pl/api/exchangerates/rates/a/usd/?format=json";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<NbpResponse>(url);

            return response?.Rates?[0].Mid;
        }
        catch (Exception ex)
        {
            //TO DO dodac logowanie
            return null;
        }
    }
}
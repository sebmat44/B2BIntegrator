namespace B2BIntegrator.App.Services;

public interface INbpApiService
{
    Task<decimal?> GetUsdExchangeRateAsync(CancellationToken cancellationToken = default);
}

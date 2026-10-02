using System.Threading.Tasks;

namespace B2BIntegrator.App.Services;

public interface INbpApiService
{
    Task<decimal?> GetUsdExchangeRateAsync();
}

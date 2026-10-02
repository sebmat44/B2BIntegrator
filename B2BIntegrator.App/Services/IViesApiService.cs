using System.Threading.Tasks;

namespace B2BIntegrator.App.Services;

public interface IViesApiService
{
    Task<(bool IsValid, string CompanyName, string Address)> VerifyVatAsync(string countryCode,
                                                                            string vatNumber);
}

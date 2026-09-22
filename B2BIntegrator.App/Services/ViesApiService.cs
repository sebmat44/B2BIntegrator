using System;
using System.Threading.Tasks;

namespace B2BIntegrator.App.Services;

public class ViesApiService
{
    public async Task<(bool IsValid, string CompanyName, string Address)> VerifyVatAsync(string countryCode, string vatNumber)
    {
        // checkVatPortTypeClient is the class generated automatically from the WSDL file
        var client = new ViesApi.checkVatPortTypeClient();

        try
        {
            var request = new ViesApi.checkVatRequest
            {
                countryCode = countryCode,
                vatNumber = vatNumber
            };

            var response = await client.checkVatAsync(request);

            return (
                response.valid,
                response.name ?? "NAME NOT PROVIDED",
                response.address ?? "ADDRESS NOT PROVIDED"
            );
        }
        catch (Exception)
        {
            return (false, string.Empty, string.Empty);
        }
        finally
        {
            // WCF clients hold network resources and must be explicitly closed or aborted
            if (client.State == System.ServiceModel.CommunicationState.Opened)
            {
                await client.CloseAsync();
            }
            else
            {
                client.Abort();
            }
        }
    }
}
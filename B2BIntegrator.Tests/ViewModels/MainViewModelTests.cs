using B2BIntegrator.App.Services;
using B2BIntegrator.App.ViewModels;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace B2BIntegrator.Tests.ViewModels;

    public class MainViewModelTests
    {

    [Fact]
    public async Task FetchUsdRateCommand_UpdatesStatusMessageWithRate()
    {
        // ARRANGE
        var mockNbp = new Mock<INbpApiService>();

        mockNbp.Setup(x => x.GetUsdExchangeRateAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(4.15m);

        var mockVies = new Mock<IViesApiService>();

        var viewModel = new MainViewModel(
            mockNbp.Object,
            mockVies.Object,
            NullLogger<MainViewModel>.Instance);

        // ACT
        await viewModel.FetchUsdRateCommand.ExecuteAsync(null);

        // ASSERT
        decimal expectedRate = 4.15m;
        string expectedRateString = expectedRate.ToString(System.Globalization.CultureInfo.InvariantCulture);

        Assert.Contains($"Aktualny kurs dolara: {expectedRateString} PLN", viewModel.StatusMessage);
    }

    [Fact]
    public void VerifyVatNumberCommand_ValidVat_SetsSuccessMessage()
    {
        // ARRANGE
        var mockNbp = new Mock<INbpApiService>();

        var mockVies = new Mock<IViesApiService>();
        mockVies.Setup(x => x.VerifyVatAsync(
            countryCode: It.IsAny<string>(),
            vatNumber: It.IsAny<string>())).ReturnsAsync((true, 
                                                          "SuperFirmaInc", 
                                                          "ul. Adresowa 1a, Warszawa"));

        var viewModel = new MainViewModel(mockNbp.Object, 
            mockVies.Object, 
            NullLogger<MainViewModel>.Instance)
        {
            VatNumber = "7740001454"
        };

        // ACT
        viewModel.VerifyVatNumberCommand.Execute(null);

        // ASSERT
        Assert.Contains("NIP POPRAWNY", viewModel.ViesResult);
        Assert.Contains("SuperFirmaInc", viewModel.ViesResult);
    }

    [Fact]
    public void VerifyVatNumberCommand_EmptyVat_SetsErrorMessage()
    {
        //AAA

        //ARRANGE
        var mockNbp = new Mock<INbpApiService>();

        var mockVies = new Mock<IViesApiService>();

        var viewModel = new MainViewModel(mockNbp.Object, 
            mockVies.Object, 
            NullLogger<MainViewModel>.Instance)
        {
            VatNumber = string.Empty
        };

        //ACT
        viewModel.VerifyVatNumberCommand.Execute(null);

        //ASSERT
        Assert.Equal("Proszę wprowadzić numer NIP.", viewModel.ViesResult);
    }
}


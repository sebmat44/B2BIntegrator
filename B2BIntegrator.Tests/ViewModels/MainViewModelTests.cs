using Xunit;
using B2BIntegrator.App.ViewModels;

namespace B2BIntegrator.Tests.ViewModels;

    public class MainViewModelTests
    {

    [Fact]
    public void VerifyVatNumberCommand_EmptyVat_SetsErrorMessage()
    {
        //AAA

        //ARRANGE
        //var viewModel = new MainViewModel();
        //viewModel.VatNumber = string.Empty;
        var viewModel = new MainViewModel
        {
            VatNumber = string.Empty
        };

        //ACT
        viewModel.VerifyVatNumberCommand.Execute(null);

        //ASSERT
        Assert.Equal("Proszę wprowadzić numer NIP.", viewModel.ViesResult);
    }
}


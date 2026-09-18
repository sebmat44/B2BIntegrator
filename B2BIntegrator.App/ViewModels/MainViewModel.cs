using System;
using System.Collections.Generic;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace B2BIntegrator.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "System gotowy do pracy";

    [RelayCommand]
    private void TestConnection()
    {
        StatusMessage = "Połączono z testowym API";
    }
}


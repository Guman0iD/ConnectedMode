using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class MainViewModel : BaseViewModel
{

    public MainViewModel()
    {
       
        WeakReferenceMessenger.Default.Register<ChangeViewModelMessage>(this,
            (sender, message) =>
            {
                CurrentViewModel = message.ViewModel;
            });

        var viewModel = App.ServiceProvider.GetService<ItemsViewModel>()!;

        var changeViewModelMessage = new ChangeViewModelMessage(viewModel);
        WeakReferenceMessenger.Default.Send(changeViewModelMessage);

        // _dbService.InitializeDb();
        // _dbService.CreateTable();
    }

    [ObservableProperty]
    private BaseViewModel? _currentViewModel;
}
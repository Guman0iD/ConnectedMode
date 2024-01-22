using System;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class AddItemViewModel : BaseViewModel
{
    public Items CurrentItem { get; }
    private readonly ViewModelFactory _factory;
    // private IConfiguration _config;
    //
    
    public AddItemViewModel(ViewModelFactory factory)//IConfiguration config)
    {
        // _config = config;
        _factory = factory;
        CurrentItem = new Items()
        {
            AddedDate = DateTime.Now
        };
    }

    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
    }

    [RelayCommand]
    private void Add()
    {
        CurrentItem.SetNextId();
        WeakReferenceMessenger.Default.Send(new AddItemMessage(this, CurrentItem));
        Back();
    }

}
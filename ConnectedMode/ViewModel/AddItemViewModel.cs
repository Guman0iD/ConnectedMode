using System;
using System.Data;
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
    private DataBaseService _dbService;
    private readonly ErrorMessage _message;

    public AddItemViewModel(ViewModelFactory factory, DataBaseService dbService)
    {
        _factory = factory;
        _dbService = dbService;
        _message = new ErrorMessage();
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
        var tableName = "Items";
        try
        {
            using var connection = _dbService.OpenConnection();
            CurrentItem.Id = _dbService.AddItem(CurrentItem, tableName);
        }
        catch (Exception e)
        {
            _message.Show("Cannot add an item to the database", "Error");
            throw;
        }
        WeakReferenceMessenger.Default.Send(new AddItemMessage(this, CurrentItem));
        Back();
    }
}
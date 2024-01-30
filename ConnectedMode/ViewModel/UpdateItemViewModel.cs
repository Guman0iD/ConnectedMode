using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;


namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class UpdateItemViewModel : BaseViewModel
{
    [ObservableProperty] 
    private Items _selectedItem = null!;
    private DataBaseService _dbService;
    private readonly ViewModelFactory _factory;
    private ErrorMessage _errorMessage;
    private JsonBase _json;

    public UpdateItemViewModel(ViewModelFactory factory, DataBaseService dbService, ErrorMessage errorMessage)
    {
        _factory = factory;
        _dbService = dbService;
        _errorMessage = errorMessage;
        _json = new JsonBase();
        WeakReferenceMessenger.Default.Register<SendItemMessage>(this,
            (sender, message) => { SelectedItem = message.UpdatedItem; });
    }

    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
    }

    [RelayCommand]
    private void Update()
    {
        try
        {
            _dbService.UpdateItemDb(SelectedItem);
            UpdateJson();
        }
        catch (Exception e)
        {
            _errorMessage.Show("Unable to update data","Error");
            throw;
        }
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
       // WeakReferenceMessenger.Default.Send(new UpdateItemMessage(SelectedItem));
    }


    private void UpdateJson()
    {
       ICollection<Items> itemsList =  _dbService.LoadItemsFromDb();
        _json.ClearFile();
        foreach (var item in itemsList)
        {
            _json.AddItemsToFile(item);
        }
    }
}
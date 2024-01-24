using System;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class UpdateItemViewModel : BaseViewModel
{
    [ObservableProperty] 
    private Items _selectedItem = null!;
    private DataBaseService _dbService;
    private readonly ViewModelFactory _factory;
    private ErrorMessage _errorMessage;

    public UpdateItemViewModel(ViewModelFactory factory, DataBaseService dbService, ErrorMessage errorMessage)
    {
        _factory = factory;
        _dbService = dbService;
        _errorMessage = errorMessage;
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
        }
        catch (Exception e)
        {
            _errorMessage.Show("Unable to update data","Error");
            throw;
        }
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
        WeakReferenceMessenger.Default.Send(new UpdateItemMessage(SelectedItem));
    }
}
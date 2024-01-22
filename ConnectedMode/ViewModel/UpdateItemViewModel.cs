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
    
    private readonly ViewModelFactory _factory;
    
    public UpdateItemViewModel(ViewModelFactory factory)
    {
        _factory = factory;
        WeakReferenceMessenger.Default.Register<UpdateItemMessage>(this, (sender, message) =>
         {
             SelectedItem = message.UpdatedItem;
         });
    }
    
    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
    }
    
    [RelayCommand]
    private void Update()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
        WeakReferenceMessenger.Default.Send(new UpdateItemMessage(SelectedItem));
    }
}
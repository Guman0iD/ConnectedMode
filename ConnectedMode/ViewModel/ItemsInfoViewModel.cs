using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class ItemsInfoViewModel : BaseViewModel
{
    private readonly ViewModelFactory _factory;
    [ObservableProperty] 
    private Items _selectedItem = null!;
    public ItemsInfoViewModel(ViewModelFactory factory)
    {
        _factory = factory;
        WeakReferenceMessenger.Default.Register<SendItemMessage>(this,
            (sender, message) =>
            {
                SelectedItem = message.UpdatedItem;
            });
    }

    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(2)));
    }
}
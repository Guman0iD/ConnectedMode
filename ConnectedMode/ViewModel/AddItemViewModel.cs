using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class AddItemViewModel : BaseViewModel
{
    public Items CurrentItem { get; }
    private readonly ViewModelFactory _factory;
    private DataBaseService _dbService;
    private readonly ErrorMessage _message;
    private JsonBase _json;

    public AddItemViewModel(ViewModelFactory factory, DataBaseService dbService)
    {
        _factory = factory;
        _dbService = dbService;
        _message = new ErrorMessage();
        _json = new JsonBase();
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
        if (string.IsNullOrEmpty(CurrentItem.ItemName) || string.IsNullOrEmpty(CurrentItem.Category)
                                                       || CurrentItem.Price == 0 || CurrentItem.Quantity == 0)
        {
            ItemNameLabel = "*";
            CategoryLabel = "*";
            PriceLabel = "*";
            QuantityLabel = "*";
            _message.Show("Fields with '*' must be filled in","Error");
            return;
        }
        try
        {
            using var connection = _dbService.OpenConnection();
            CurrentItem.Id = _dbService.AddItem(CurrentItem);
            _json.AddItemsToFile(CurrentItem);
        }
        catch (Exception e)
        {
            _message.Show("Cannot add an item to the database", "Error");
            throw;
        }

      //  WeakReferenceMessenger.Default.Send(new AddItemMessage(this, CurrentItem));
        Back();
    }

    [ObservableProperty] 
    private string _itemNameLabel = null!;
    [ObservableProperty] 
    private string _categoryLabel = null!;
    [ObservableProperty] 
    private string _priceLabel = null!;
    [ObservableProperty] 
    private string _quantityLabel = null!;

}
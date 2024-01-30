using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
public partial class ItemsViewModel : BaseViewModel
{
    public ICollection<Items?> ItemsList { get; }
    public ICollection<Items?> SearchedItems { get; }

    private readonly ViewModelFactory _factory;
    private ErrorMessage _message;
    private DataBaseService _dbService;

    [ObservableProperty] private string? _searchKeyword;
    [ObservableProperty] private int _quantityCount;

    public ItemsViewModel(ViewModelFactory factory, DataBaseService dbService)
    {
        _factory = factory;
        _dbService = dbService;
        _message = new ErrorMessage();
        ItemsList = new ObservableCollection<Items?>();
        SearchedItems = new ObservableCollection<Items?>();
        LoadStudents();
        QuantityCount = _dbService.GetCount();
    }


    private void LoadStudents()
    {
        try
        {
            var itemsFromDb = _dbService.LoadItemsFromDb();
            foreach (var item in itemsFromDb)
            {
                ItemsList.Add(item);
            }
        }
        catch (Exception e)
        {
            _message.Show($"Error loading items: {e.Message}", "Error");
            throw;
        }
    }


    [RelayCommand]
    private void AddItem()
    {
        WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(1)));
    }

    [RelayCommand(CanExecute = "CanRemove")]
    private void Delete(object? param)
    {
        if (param is Items item)
        {
            ItemsList.Remove(item);
            SearchedItems.Remove(item);
            JsonBase json = new JsonBase();
            json.RemoveItemFromFile(item);
            try
            {
                _dbService.RemoveFromDb(item.Id);
            }
            catch (Exception e)
            {
                _message.Show($"Can't remove item from db{e.Message}", "Error");
                throw;
            }

            QuantityCount = _dbService.GetCount();
        }
    }

    private bool CanRemove => ItemsList.Count > 0;
    private bool CanUpdate => ItemsList.Count > 0;

    [RelayCommand]
    private void Search()
    {
        SearchedItems.Clear();
        int id;
        if (int.TryParse(SearchKeyword, out id))
        {
            var item = ItemsList.FirstOrDefault(i => i?.Id == id);
            if (item != null)
            {
                SearchedItems.Add(item);
            }
        }
        else
        {
            var filteredItems = ItemsList.Where(i =>
                i?.ItemName?.Contains(SearchKeyword!, StringComparison.OrdinalIgnoreCase) == true ||
                i?.Category.Contains(SearchKeyword!, StringComparison.OrdinalIgnoreCase) == true);
            foreach (var item in filteredItems)
            {
                SearchedItems.Add(item);
            }
        }

        SearchKeyword = string.Empty;
    }

    [RelayCommand(CanExecute = "CanUpdate")]
    private void UpdateItem(object? param)
    {
        if (param is Items item)
        {
            WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(3)));
            WeakReferenceMessenger.Default.Send(new SendItemMessage(item));
        }
    }

    private bool CanGetDescription => SearchedItems.Count > 0;

    [RelayCommand(CanExecute = "CanGetDescription")]
    private void Description(object? param)
    {
        if (param is Items item)
        {
            WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(4)));
            WeakReferenceMessenger.Default.Send(new SendItemMessage(item));
        }
    }
}
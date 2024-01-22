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
    public ObservableCollection<Items?> ItemsList { get; }
    public ObservableCollection<Items?> SearchedItems { get; }
    private readonly ViewModelFactory _factory;
    private readonly IConfiguration _config;

    [ObservableProperty] 
    private string _searchKeyword;

    public ItemsViewModel(IConfiguration config, ViewModelFactory factory)
    {
        _config = config;
        _factory = factory;
        ItemsList = new ObservableCollection<Items?>();
        SearchedItems = new ObservableCollection<Items?>();
        JsonAdd json = new JsonAdd();
        List<Items> itemsFromFile = json.GetItemsFromFile();

        foreach (var game in itemsFromFile)
        {
            ItemsList.Add(game);
        }
        
        WeakReferenceMessenger.Default.Register<AddItemMessage>(this,
            (sender, message) =>
            {
                ItemsList.Add(message.Items);
                json.AddItemsToFile(message.Items);
            });
    
        WeakReferenceMessenger.Default.Register<UpdateItemMessage>(this,
            (sender, message) =>
            {
                ItemsList.Remove(ItemsList.FirstOrDefault(i => i.Id == message.UpdatedItem.Id));
                ItemsList.Add(message.UpdatedItem);
            });
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

            JsonAdd json = new JsonAdd();
            json.RemoveItemFromFile(item);  
        }
    }

    private bool CanRemove => ItemsList.Count > 0;
   
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
                i?.ItemName?.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase) == true ||
                i?.Category.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase) == true);
            foreach (var item in filteredItems)
            {
                SearchedItems.Add(item);
            }
        }

        SearchKeyword = string.Empty;
    }

    [RelayCommand]
    private void UpdateItem(object? param)
    {
        if (param is Items item)
        {
            WeakReferenceMessenger.Default.Send(new ChangeViewModelMessage(_factory.Create(3)));
            WeakReferenceMessenger.Default.Send(new UpdateItemMessage(item));
        }
    }
}
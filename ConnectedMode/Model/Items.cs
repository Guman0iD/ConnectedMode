using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ConnectedMode.Model;

public partial class Items : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty] 
    private string _itemName = null!;
    [ObservableProperty] 
    private int _quantity;
    [ObservableProperty] 
    private string _category = null!;
    [ObservableProperty] 
    private decimal _price;
    [ObservableProperty]
    private string? _description ;
    [ObservableProperty] 
    private DateTime _addedDate ;
}
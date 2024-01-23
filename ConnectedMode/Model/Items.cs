using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ConnectedMode.Model;

public partial class Items : ObservableObject
{
    private static int _idCounter = 0;

    public Items()
    {
        
    }

    private static int GetNextId()
    {
        return ++_idCounter;
    }
    public void SetNextId()
    {
        Id = GetNextId();
    }
    
    
    [ObservableProperty]
    private int _id;
    [ObservableProperty] 
    private string _itemName = null!;
    [ObservableProperty] 
    private int _quantity;
    [ObservableProperty] 
    private string _category = null!;
    [ObservableProperty] 
    private int _price;
    [ObservableProperty]
    private string? _description ;
    [ObservableProperty] 
    private DateTime _addedDate ;
}
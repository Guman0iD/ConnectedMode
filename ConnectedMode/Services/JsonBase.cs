using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ConnectedMode.Model;
using Newtonsoft.Json;


namespace ConnectedMode.Services;

public class JsonBase
{
    private const string _filePath = "items.json";
    
    public void AddItemsToFile(Items items)
    {
        List<Items> existingItems = new List<Items>();

        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            existingItems = JsonConvert.DeserializeObject<List<Items>>(json) ?? new List<Items>();
        }

        existingItems.Add(items);

        string updatedJson = JsonConvert.SerializeObject(existingItems, Formatting.Indented);
        File.WriteAllText(_filePath, updatedJson);
    }
    
    public List<Items> GetItemsFromFile()
    {
        List<Items> existingItems = new List<Items>();

        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            existingItems = JsonConvert.DeserializeObject<List<Items>>(json) ?? new List<Items>();
        }

        return existingItems;
    }
    public void RemoveItemFromFile(Items items)
    {
        List<Items> existingItems = GetItemsFromFile();
    
        Items itemToRemove = existingItems.FirstOrDefault(i => i.ItemName == items.ItemName && i.Category == items.Category)!;
        {
            existingItems.Remove(itemToRemove);
    
            string updatedJson = JsonConvert.SerializeObject(existingItems, Formatting.Indented);
            File.WriteAllText(_filePath, updatedJson);
        }
    }
    
    public void ClearFile()
    {
        File.WriteAllText(_filePath, string.Empty);
    }
}
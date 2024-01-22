using ConnectedMode.Model;

namespace ConnectedMode.Messages;

public class AddItemMessage
{
    public AddItemMessage(object sender, Items items)
    {
        Items = items;
    }
    public Items Items { get; }
}
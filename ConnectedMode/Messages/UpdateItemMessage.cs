using ConnectedMode.Model;

namespace ConnectedMode.Messages;

public class UpdateItemMessage
{
 public Items UpdatedItem { get; private set; }

    public UpdateItemMessage(Items updatedItem)
    {
        UpdatedItem = updatedItem.Clone();
    }
}
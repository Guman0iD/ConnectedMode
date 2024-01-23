using ConnectedMode.Model;

namespace ConnectedMode.Messages;

public class SendItemMessage
{
    public Items UpdatedItem { get; private set; }

    public SendItemMessage(Items updatedItem)
    {
        UpdatedItem = updatedItem;
    }
}
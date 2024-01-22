using System.Windows.Forms;

namespace ConnectedMode.Services;

public interface IMessageBox
{
    void Show(string text, string header);
}
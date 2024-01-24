using System.Windows;

namespace ConnectedMode.Services;

public class ErrorMessage : IMessageBox
{
    public void Show(string text, string header)
    {
        MessageBox.Show(text,header,MessageBoxButton.OK,MessageBoxImage.Error);
    }
}
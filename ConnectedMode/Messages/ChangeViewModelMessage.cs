using ConnectedMode.ViewModel;

namespace ConnectedMode.Messages;

public class ChangeViewModelMessage
{
    public ChangeViewModelMessage( BaseViewModel viewModel) 
    {
        ViewModel = viewModel;
    }
    public BaseViewModel ViewModel { get;}
}